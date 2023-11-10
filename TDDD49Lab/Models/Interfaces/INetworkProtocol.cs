using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Interfaces
{
    public interface INetworkProtocol<T>  where T : struct
    {

        Task<string> CreateMessageAsync(string message);

        Task<string> CreateConnectionRequestStringAsync(string username);

        Task<string> CreateDisconnectStringAsync(string username);

        Task<T> DecodeStringeAsync(string recivedString);   


    }
}
