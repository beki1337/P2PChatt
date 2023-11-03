using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace TDDD49Lab.Models
{
    internal class ChattHistory
    {
        private readonly string directoryPath = @"C:\ChattConservationTddd49\";
        private readonly string fileType = "*.json";

        public List<ConservationFilename>  GetConneversations()
        {
           
                string[] files = Directory.GetFiles(directoryPath, fileType);
                string[] fileNames = files.Select(filePath => Path.GetFileName(filePath)).ToArray();

                List<ConservationFilename> conservationFilenames = new List<ConservationFilename>();
                foreach (string file in fileNames)
                {
                    conservationFilenames.Add(new ConservationFilename(file));
                }
                return conservationFilenames.OrderByDescending(x => x.ConnverstionHappend).ToList();

        }



        public ObservableCollection<Message> GetMessages(string fileName)
        {
           if(!File.Exists(directoryPath+fileName + ".json"))
           {
                MessageBox.Show($"Could not find file {directoryPath + fileName + ".json"}");
           }
         
            string conservationJson = File.ReadAllText(directoryPath+fileName+".json");

            ConservationJsonSyntaxt deserializedData = JsonSerializer.Deserialize<ConservationJsonSyntaxt>(conservationJson) ?? throw new Exception("df");
            return new ObservableCollection<Message>(deserializedData.Messages);
        }

    }
}
