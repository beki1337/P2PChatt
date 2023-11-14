using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    internal class DataSerializer<T> : IDataSerialize<T>
    {

        public string SerializeToFormat(T obj)
        {
            // Implementation for serialization to any format
            return JsonSerializer.Serialize(obj);
        }

        public T DeserializeFromFormat(string data)
        {
            // Implementation for deserialization from any format
            return JsonSerializer.Deserialize<T>(data);
        }

      
    }
}
