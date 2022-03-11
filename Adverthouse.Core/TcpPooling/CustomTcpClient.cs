using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Core.TcpPooling
{
    public class CustomTcpClient : TcpClient
    {
        public DateTime TimeCreated { get; private set; }

        public CustomTcpClient(string host, int port) : base(host, port)
        {
            TimeCreated = DateTime.Now;
            Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.KeepAlive, false);
            Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 1);
            Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 5);
            Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 1);
        }
    }
}
