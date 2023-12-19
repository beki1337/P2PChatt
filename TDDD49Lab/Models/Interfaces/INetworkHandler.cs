using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDDD49Lab.Models.Enums;

namespace TDDD49Lab.Models.Interfaces
{
    public interface INetworkHandler<T> : IDisposable where T : IMessage
    {
        Task<T> Connect(string ipAddress, int port);
        Task<bool> AcceptConnection();
        Task DeclineRequest();
        Task<T> Listen(string IpAddres, int port);
        IAsyncEnumerable<T?> GetMessages([EnumeratorCancellation] CancellationToken cancellationToken = default);
        Task SendMessage(string message);
        Task Disconect();

        NetworkState CurrentNetworkStatet();
    }
}
