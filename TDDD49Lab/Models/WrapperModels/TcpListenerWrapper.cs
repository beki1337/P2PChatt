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
   public  class TcpListenerWrapper : ITcpListener
    {
        private TcpListener tcpListener;

       public TcpListenerWrapper(string ipAddress, int port)
        {
            tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(ipAddress), port));

        }

        public async Task<TcpClient> AcceptTcpClientAsync()
        {
            return await tcpListener.AcceptTcpClientAsync();
        }

     
        public void Start()
        {
            tcpListener.Start();
        }

        public void Stop()
        {
            tcpListener.Stop();
        }
    }
}
