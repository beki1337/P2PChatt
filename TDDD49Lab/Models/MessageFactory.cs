using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models.Enums;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    public class MessageFactory : INetworkProtocol<IMessage>
    {
        public IMessage CreateMessage(string username, string message)
        {
            return new Message(username,message,true,DateTime.Now,NetworkProtocolTypes.Message);
        }

        public IMessage CreateConnectionHandshake(string username)
        {
            return new Message(username,"",true,DateTime.Now,NetworkProtocolTypes.EstablishConnection);
        }

        public IMessage CreateDeclineHandshake(string username)
        {
            return new Message(username,"",true,DateTime.Now,NetworkProtocolTypes.DeniedConnection );
        }

        public IMessage CreateAcceptHandshake(string username)
        {
            return new Message(username,"",true,DateTime.Now,NetworkProtocolTypes.AcceptConnection);
        }

        public IMessage CreateDisconnect(string username)
        {
            return new Message(username,"",true,DateTime.Now,NetworkProtocolTypes.Disconnect);
        }

        public IMessage CreateBuzz(string username)
        {
            return new Message(username,"",true,DateTime.Now,NetworkProtocolTypes.Buzz);
        }



    }
}
