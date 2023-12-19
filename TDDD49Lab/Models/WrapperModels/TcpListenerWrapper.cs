using System.Net;
using System.Net.Sockets;
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

        public async Task<ITcpClient> AcceptTcpClientAsync()
        {
            TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

            return new TcpClientWrapper(tcpClient);
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
