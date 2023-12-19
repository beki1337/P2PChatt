using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    public class DataSerializer<T> : IDataSerialize<T> where T : IMessage
    {

        public string SerializeToFormat(T obj)
        {
            // Implementation for serialization to any format
            return JsonSerializer.Serialize(obj);
        }

        public T? DeserializeFromFormat<TConcrete>(string? data) where TConcrete :class, IMessage
        {
            if (data == null)
            {
                // Handle the case where data is null
                // You might want to throw an exception or handle it based on your requirements
                throw new ArgumentNullException(nameof(data));
            }

            // Implementation for deserialization from any format
        
            IMessage dd = JsonSerializer.Deserialize<TConcrete>(data);
            
            return (T)dd;
        }

      
    }
}
