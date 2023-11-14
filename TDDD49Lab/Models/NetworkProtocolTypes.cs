using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models
{
    public enum NetworkProtocolTypes
    {
        Message,
        EstablishConnection,
        Buzz,
        Disconnect,
        DeniedConnection,
        AcceptConnection
    }
}
