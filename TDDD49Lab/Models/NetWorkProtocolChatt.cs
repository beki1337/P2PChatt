using Microsoft.Windows.Themes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models
{
    internal class NetWorkProtocolChatt
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }


        public NetWorkProtocolChatt(NetworkProtocolTypes type, string username, string message)
        {
            Type = getProctcollType(type);
            Username = username;
            Message = message;
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string getProctcollType(NetworkProtocolTypes type) => type switch
        {
            NetworkProtocolTypes.Message => "message",
            NetworkProtocolTypes.EstablishConnection => "establishconnection",
            NetworkProtocolTypes.Buzz => "buzz",
            NetworkProtocolTypes.Disconnect => "disconnect",
            NetworkProtocolTypes.DeniedConnection => "deniedconnection",
            
            _ => throw new ArgumentException("The provided type does not exist"),
        };

        public string ToJson()
        {
            Dictionary<string, string> jsonObject = new Dictionary<string, string>
            {
            { "type", Type },
            { "date", Date },
            { "username", Username },
            { "message", Message }
            };

          
            return JsonConvert.SerializeObject(jsonObject);
        }
        public static NetWorkProtocolChatt FromJson(string jsonString)
        {
            
            
             return JsonConvert.DeserializeObject<NetWorkProtocolChatt>(jsonString) ?? throw new Exception("The return value will be be null");
            
        }
            
        
        }

    
}
