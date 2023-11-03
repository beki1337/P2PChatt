using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Interfaces
{
   public  interface ITcpClient
    {
        /// <summary>
        /// Connects the client to a remote TCP host using the specified endpoint as an asynchronous operation.
        /// </summary>
        /// <param name="remoteEP">The <see cref="IPEndPoint"/> to which you intend to connect.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ConnectAsync(IPEndPoint remoteEP);

        /// <returns>The stream used to read and write data to the remote host.</returns>
        Stream GetStream();
        void Dispose();
    }
}
