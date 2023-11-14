using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Interfaces
{
    public interface INetworkProtocol<T>  where T : IMessage
    {

        T CreateMessage(string username, string message);

        T CreateConnectionHandshake(string username);

        T CreateDeclineHandshake(string username);

        T CreateAcceptHandshake(string username);

        T CreateDisconnect(string username);

        T CreateBuzz(string username);

         

    }
}
