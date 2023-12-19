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
    public class MessageFactory: INetworkProtocol<IMessage>
    {
        private IDateTime dateTime;
        

        public MessageFactory(IDateTime dateTime) {
            this.dateTime = dateTime;
        }

        public IMessage CreateMessage(string username, string message)
        {
            return new Message(username,message,true, dateTime.Now, NetworkProtocolTypes.Message);
        }

        public IMessage CreateConnectionHandshake(string username)
        {
            return new Message(username,"",true, dateTime.Now, NetworkProtocolTypes.EstablishConnection);
        }

        public IMessage CreateDeclineHandshake(string username)
        {
            return new Message(username,"",true, dateTime.Now, NetworkProtocolTypes.DeniedConnection );
        }

        public IMessage CreateAcceptHandshake(string username)
        {
            return new Message(username,"",true, dateTime.Now, NetworkProtocolTypes.AcceptConnection);
        }

        public IMessage CreateDisconnect(string username)
        {
            return new Message(username,"",true, dateTime.Now, NetworkProtocolTypes.Disconnect);
        }

        public IMessage CreateBuzz(string username)
        {
            return new Message(username,"",true, dateTime.Now, NetworkProtocolTypes.Buzz);
        }

        
    }
}
