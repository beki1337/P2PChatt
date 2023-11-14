using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    public class Conservation<T> : IConservation<T>
    {
        private readonly List<T> messages = new List<T>();
 
        private readonly Stream outputStream;

        private IDataSerialize<T> dataSerializeToFormat;

        public Conservation(Stream stream, IDataSerialize<T> serializer) {
            this.outputStream = stream;
            this.dataSerializeToFormat = serializer;
        }

        public async Task AddMessage(T message)
        {
            messages.Add(message);
            if (this.messages.Count >= 10)
            {
                await SendToWritteAsync();
            }
            
        }

        private async Task SendToWritteAsync()
        {

            using (StreamWriter streamWriter = new StreamWriter(outputStream))
            {
                // Combine the directory path and file name to create the full file path
                foreach (var message in messages)
                {

                    string serilazedObject = dataSerializeToFormat.SerializeToFormat(message);

                    await streamWriter.WriteAsync(serilazedObject);

                }
                await streamWriter.FlushAsync();
            }
            messages.Clear();
           
        }

    }
}
