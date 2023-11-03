using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TDDD49Lab.Models
{
    internal class ConservationJsonSyntaxt
    {
        [JsonPropertyName("Messages")]
        public List<Message> Messages { get; set; }

        [JsonPropertyName("YourName")]
        public string YourName { get; set; }

        [JsonPropertyName("OtherUserName")]
        public string OtherUserName { get; set; }

       
        public ConservationJsonSyntaxt(List<Message> messages,string yourName, string otherUserName) { 
            this.Messages = messages;
            this.YourName = yourName;
            this.OtherUserName = otherUserName; 
        }

    }
}
