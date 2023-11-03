using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TDDD49Lab.Models
{
    public class Message
    {
        [JsonPropertyName("Username")]
        public string Username { get; set; }

        [JsonPropertyName("MessageText")]
        public string MessageText { get; set; }

        [JsonPropertyName("DateTime")]
        public DateTime DateTime { get; set; }

        [JsonPropertyName("IsYourMessage")]
        public bool IsYourMessage { get; set; }

        public Message(string Username, string MessageText, bool IsYourMessage, DateTime DateTime)
        {
            this.Username = Username;
            this.MessageText = MessageText;
            this.DateTime = DateTime;
            this.IsYourMessage = IsYourMessage;
            
        }


    }
}
