using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Interfaces
{
    public interface ITcpListener 
    {
        Task<ITcpClient> AcceptTcpClientAsync();

        void Stop();
        void Start();
    }
}
