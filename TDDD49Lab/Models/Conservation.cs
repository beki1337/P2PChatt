using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace TDDD49Lab.Models
{
    internal class Conservation
    {
        private List<Message> messages;

        private string otheruser = "Deuflr User";

        private readonly string directoryPath = @"C:\ChattConservationTddd49";

        private JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

        public Conservation( string otheruser) { 
            messages = new List<Message>();
       
            this.otheruser = otheruser;
            CreateDirectory(directoryPath);
        }

        public void AddMessage(Message message)
        {
            messages.Add(message);
            
        }

        public async Task SendToWritte()
        {
            // Replace with your desired directory
            string fileName = createFileName(otheruser);

            // Combine the directory path and file name to create the full file path
            string filePath = Path.Combine(directoryPath, fileName);
            string convservationJson = JsonSerializer.Serialize(new ConservationJsonSyntaxt(messages, "listen", "sdfdsf"));
            MessageBox.Show(convservationJson);
            await File.WriteAllTextAsync(filePath, convservationJson);
            messages.Clear();
            MessageBox.Show("We have called the add ");
        }
        

        private string createFileName(string otherUser)
        {
            DateTime now = DateTime.Now;

            // Format the DateTime as a string that can be used as a file name
            string formattedFileName = now.ToString("yyyy-MM-dd HH-mm-ss");
            return $"{otheruser}-{formattedFileName}.json";
        }


        private void CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }


    }
}
