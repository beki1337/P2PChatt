using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Interfaces
{
    public interface IMessage
    {
        string Username { get; }
        string MessageText { get; }
        DateTime DateTime { get; }
        bool IsYourMessage { get; }
        NetworkProtocolTypes NetworkProtocolType { get; }

    }
}
