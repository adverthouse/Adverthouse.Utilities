using System;
using System.Collections.Generic;

namespace Adverthouse.Core.SocketPooling
{
    public class ServerPool
    {
        //Expose the socket pools.
        private SocketPool[] HostList { get; set; }

        private Dictionary<uint, SocketPool> hostDictionary;
        private uint[] hostKeys;

        public int SendReceiveTimeout { get; set; } = 2000;
        public uint MaxPoolSize { get; set; } = 10;
        public uint MinPoolSize { get; set; } = 5;
        public TimeSpan SocketRecycleAge { get; set; } = TimeSpan.FromMinutes(30);


        private delegate T UseSocket<T>(PooledSocket socket);
        private delegate void UseSocket(PooledSocket socket);
        /// <summary>
        /// private constructor. This method takes the array of hosts and sets up an private list of socketpools.
        /// </summary>
        public ServerPool(string[] hosts)
        {
            hostDictionary = new Dictionary<uint, SocketPool>();
            List<SocketPool> pools = new List<SocketPool>();
            List<uint> keys = new List<uint>();
            foreach (string host in hosts)
            {
                //Create pool
                SocketPool pool = new SocketPool(this, host.Trim());

                //Create 250 keys for this pool, store each key in the hostDictionary, as well as in the list of keys.
                for (int i = 0; i < 250; i++)
                {
                    uint key = (uint)i;
                    if (!hostDictionary.ContainsKey(key))
                    {
                        hostDictionary[key] = pool;
                        keys.Add(key);
                    }
                }

                pools.Add(pool);
            }

            //Hostlist should contain the list of all pools that has been created.
            HostList = pools.ToArray();

            //Hostkeys should contain the list of all key for all pools that have been created.
            //This array forms the server key continuum that we use to lookup which server a
            //given item key hash should be assigned to.
            keys.Sort();
            hostKeys = keys.ToArray();
        }

        /// <summary>
        /// Given an item key hash, this method returns the serverpool which is closest on the server key continuum.
        /// </summary>
        private SocketPool GetSocketPool(uint hash)
        {
            //Quick return if we only have one host.
            if (HostList.Length == 1) return HostList[0];

            //New "ketama" host selection.
            int i = Array.BinarySearch(hostKeys, hash);

            //If not exact match...
            if (i < 0)
            {
                //Get the index of the first item bigger than the one searched for.
                i = ~i;

                //If i is bigger than the last index, it was bigger than the last item = use the first item.
                if (i >= hostKeys.Length)
                {
                    i = 0;
                }
            }
            return hostDictionary[hostKeys[i]];
        }

        private SocketPool GetSocketPool(string host)
        {
            return Array.Find(HostList, delegate (SocketPool socketPool) { return socketPool.Host == host; });
        }

        /// <summary>
        /// This method executes the given delegate on a socket from the server that corresponds to the given hash.
        /// If anything causes an error, the given defaultValue will be returned instead.
        /// This method takes care of disposing the socket properly once the delegate has executed.
        /// </summary>
        private T Execute<T>(uint hash, T defaultValue, UseSocket<T> use)
        {
            return Execute(GetSocketPool(hash), defaultValue, use);
        }

        private T Execute<T>(SocketPool pool, T defaultValue, UseSocket<T> use)
        {
            PooledSocket sock = null;
            try
            {
                //Acquire a socket
                sock = pool.Acquire();

                //Use the socket as a parameter to the delegate and return its result.
                if (sock != null)
                {
                    return use(sock);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Execute<T>: " + pool.Host);

                //Socket is probably broken
                if (sock != null)
                {
                    sock.Close();
                }
            }
            finally
            {
                if (sock != null)
                {
                    sock.Dispose();
                }
            }
            return defaultValue;
        }

        private void Execute(SocketPool pool, UseSocket use)
        {
            PooledSocket sock = null;
            try
            {
                //Acquire a socket
                sock = pool.Acquire();

                //Use the socket as a parameter to the delegate and return its result.
                if (sock != null)
                {
                    use(sock);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Execute: " + pool.Host);

                //Socket is probably broken
                if (sock != null)
                {
                    sock.Close();
                }
            }
            finally
            {
                if (sock != null)
                {
                    sock.Dispose();
                }
            }
        }

        /// <summary>
        /// This method executes the given delegate on all servers.
        /// </summary>
        private void ExecuteAll(UseSocket use)
        {
            foreach (SocketPool socketPool in HostList)
            {
                Execute(socketPool, use);
            }
        }
    }
}
