using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    public readonly struct Message : IMessage
    {
        [JsonPropertyName("Username")]
        public string Username { get; }

        [JsonPropertyName("MessageText")]
        public string MessageText { get; }

        [JsonPropertyName("DateTime")]
        public DateTime DateTime { get; }

        [JsonPropertyName("IsYourMessage")]
        public bool IsYourMessage { get;  }

        [JsonPropertyName("NetworkProtocolType")]
        public NetworkProtocolTypes NetworkProtocolType { get; }

        public Message(string Username, string MessageText, bool IsYourMessage, DateTime DateTime, NetworkProtocolTypes NetworkProtocolType)
        {
            this.Username = Username;
            this.MessageText = MessageText;
            this.DateTime = DateTime;
            this.IsYourMessage = IsYourMessage;
            this.NetworkProtocolType = NetworkProtocolType;
            
        }


    }
}
