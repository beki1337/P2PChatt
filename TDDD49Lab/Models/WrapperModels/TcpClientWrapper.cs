using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models.WrapperModels
{
    class TcpClientWrapper:ITcpClient
    {
        private readonly TcpClient tcpClient;

        public TcpClientWrapper()
        {
            tcpClient = new TcpClient();
        }

        public Task ConnectAsync(IPEndPoint remoteEP)
        {
            return tcpClient.ConnectAsync(remoteEP);
        }

        public void Dispose()
        {
            tcpClient.Dispose();
        }

        public NetworkStream GetStream()
        {
            return tcpClient.GetStream();
        }
    }
}
