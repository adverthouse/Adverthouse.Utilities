using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Core.Sockets
{
    public class AsynchronousClient
    {
        private static int port = 3866;
        private static readonly object syncLock = new object();
        private static readonly object deleteLock = new object();
        private static ConcurrentDictionary<int, StateObject> clients = new();
        private static int TotalConnectionCount = 0;
        private static string IPAddress = "";

        public AsynchronousClient(string ipAddress, int portNumber)
        {
            IPAddress = ipAddress;
            port = portNumber;
        }

        private static StateObject GetConnection()
        {
            var usableConnections = clients.Where(a => a.Value.ConnectionStatus == ConnectionState.Idle);
            if (usableConnections.Count() > 0)
            {
                var soe = usableConnections.First();
                soe.Value.receiveDone.Set();
                soe.Value.ConnectionStatus = ConnectionState.Connected;
                return soe.Value;
            }

            IPHostEntry ipHostInfo = Dns.GetHostEntry(IPAddress);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            var so = new StateObject()
            {
                workSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            };

            so.workSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            so.workSocket.ExclusiveAddressUse = false;
            so.ConnectionID = TotalConnectionCount;
            TotalConnectionCount++;

            clients.TryAdd(so.ConnectionID, so);
            clients[so.ConnectionID].workSocket.BeginConnect(remoteEP, ConnectCallback, so);
            clients[so.ConnectionID].connectDone.WaitOne();

            return clients[so.ConnectionID];
        }
        public static int ActiveConnectionCount
        {
            get
            {
                return clients.Where(a => a.Value.ConnectionStatus == ConnectionState.Connected).Count();
            }
        }

        public static T? SendCommand<T>(string command)
        {
            StateObject state = null;

            lock (syncLock)
            {
                state = GetConnection();
            }



            byte[] byteData = Encoding.UTF8.GetBytes(command + "<EOF>");

            state.workSocket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, state);
            state.sendDone.WaitOne();

            state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
            state.receiveDone.WaitOne();

            clients[state.ConnectionID].ConnectionStatus = ConnectionState.Idle;

            if (state.result == null) return default(T);
            return JsonConvert.DeserializeObject<T>(state.result);
        }

        public static void Disconnnect(int ConnectionID)
        {
            lock (deleteLock)
            {
                clients[ConnectionID].workSocket.Shutdown(SocketShutdown.Both);
                clients[ConnectionID].workSocket.Close();
            }
        }
        public static void DisconnectAll()
        {
            foreach (var client in clients)
            {
                client.Value.workSocket.Shutdown(SocketShutdown.Both);
            }
            clients.Clear();
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                state.workSocket.EndConnect(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                state.connectDone.Set();
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;

            try
            {
                Socket client = state.workSocket;
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                    if (bytesRead >= state.buffer.Length)
                    {
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
                    }
                }

                if (state.sb.Length > 1)
                {
                    state.result = state.sb.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                state.receiveDone.Set();
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;

            try
            {
                int bytesSent = state.workSocket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                state.sendDone.Set();
            }
        }
    }
}
