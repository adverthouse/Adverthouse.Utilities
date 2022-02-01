using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using System.Threading.Tasks;

namespace Adverthouse.Core.Sockets
{
    public class StateObject
    {
        public int ConnectionID { get; set; } = 0;
        public int RequestCount { get; set; } = 0;
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data .  
        public StringBuilder sb = new StringBuilder();

        public String result = null;
        public ManualResetEvent connectDone = new ManualResetEvent(false);
        public ManualResetEvent sendDone = new ManualResetEvent(false);
        public ManualResetEvent receiveDone = new ManualResetEvent(false);

        public ConnectionState ConnectionStatus = ConnectionState.Connected;
    }

}
