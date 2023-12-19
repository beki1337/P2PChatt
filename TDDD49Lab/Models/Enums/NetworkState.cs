using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Enums
{
    public enum NetworkState
    {
        Unknown,
        Ideal,
        WaitingForHandshake,
        Searching,
        Listening,
        Connected,

    }
}
