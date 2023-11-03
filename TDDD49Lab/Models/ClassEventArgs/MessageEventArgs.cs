using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.ClassEventArgs
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(string username,string message)
        {
            Username = username;
            Message = message;
        }

        public string Username { get; }

        public string Message { get; }
    }
}
