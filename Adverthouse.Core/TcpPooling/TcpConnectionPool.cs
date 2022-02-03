using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Core.TcpPooling
{
    public static class TcpConnectionPool
    {
        /// <summary>
        /// Queue of available socket connections.
        /// </summary>
        private static Queue<CustomTcpClient> availableSockets = null;
        /// <summary>
        /// host IP Address
        /// </summary>
        private static string hostIP = string.Empty;
        /// <summary>
        /// host Port
        /// </summary>
        private static int hostPort = 0;
        /// <summary>
        /// Initial number of connections
        /// </summary>
        private static int POOL_MIN_SIZE = 5;
        /// <summary>
        /// The maximum size of the connection pool.
        /// </summary>
        private static int POOL_MAX_SIZE = 20;


        public static bool Initialized = false;
        public const bool ConsoleWriteMessage = false;

        private static int SOCKET_LIFE_TIME_AS_MINUTE = 15;

        /// <summary>
        /// Initialize host Connection pool
        /// </summary>
        /// <param name="hostIP">host IP Address</param>
        /// <param name="hostPort">host Port</param>
        /// <param name="minConnections">Initial number of connections</param>
        /// <param name="maxConnections">The maximum size of the connection pool</param>
        public static void InitializeConnectionPool(string hostIPAddress, int hostPortNumber, int minConnections, int maxConnections, int socketLifeTimeAsMinute = 15)
        {
            POOL_MAX_SIZE = maxConnections;
            POOL_MIN_SIZE = minConnections;
            hostIP = hostIPAddress;
            hostPort = hostPortNumber;
            SOCKET_LIFE_TIME_AS_MINUTE = socketLifeTimeAsMinute;
            availableSockets = new Queue<CustomTcpClient>();

            for (int i = 0; i < minConnections; i++)
            {
                CustomTcpClient cachedSocket = OpenSocket();
                PutSocket(cachedSocket);
            }

            Initialized = true;
        }

        /// <summary>
        /// Get an open socket from the connection pool.
        /// </summary>
        /// <returns>Socket returned from the pool or new socket opened. </returns>
        public static CustomTcpClient GetSocket()
        {
            if (TcpConnectionPool.availableSockets.Count > 0)
            {
                lock (availableSockets)
                {
                    while (TcpConnectionPool.availableSockets.Count > 0)
                    {
                        var socket = TcpConnectionPool.availableSockets.Dequeue();

                        if (socket.Connected)
                            return socket;
                        else DisposeSocket(socket);
                    }
                }
            }

            return TcpConnectionPool.OpenSocket();
        }

        /// <summary>
        /// Return the given socket back to the socket pool.
        /// </summary>
        /// <param name="socket">Socket connection to return.</param>
        public static void PutSocket(CustomTcpClient socket)
        {
            lock (availableSockets)
            {
                TimeSpan socketLifeTime = DateTime.Now.Subtract(socket.TimeCreated);

                if (TcpConnectionPool.availableSockets.Count < TcpConnectionPool.POOL_MAX_SIZE && socketLifeTime.Minutes < SOCKET_LIFE_TIME_AS_MINUTE)
                {
                    if (socket != null)
                    {
                        if (socket.Connected)
                        {
                            TcpConnectionPool.availableSockets.Enqueue(socket);
                        }
                        else DisposeSocket(socket);
                    }
                }
                else DisposeSocket(socket);
            }
        }

        private static void DisposeSocket(CustomTcpClient socket)
        {
            socket.Close();
            socket.Dispose();
        }

        public static void DisposeAllSockets()
        {
            foreach (var socket in TcpConnectionPool.availableSockets)
            {
                socket.Close();
                socket.Dispose();
            }
        }
        /// <summary>
        /// Open a new socket connection.
        /// </summary>
        /// <returns>Newly opened socket connection.</returns>
        private static CustomTcpClient OpenSocket()
        {
            lock (availableSockets)
            {
                while (TcpConnectionPool.availableSockets.Count >= POOL_MAX_SIZE)
                {
                    Console.WriteLine("Waiting for existing sockets close");
                    System.Threading.Thread.Sleep(100);
                }
            }

            return new CustomTcpClient(hostIP, hostPort);
        }
    }
}