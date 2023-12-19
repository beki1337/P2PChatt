using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    public class HistoryConservation<T> : IHistoryConservation<T>
    {

        private readonly string directoryPath;
        private readonly string fileType;

        private readonly IDataSerialize<T> dataSerializeToFormat;
        public HistoryConservation(IDataSerialize<T> dataSerializeToFormat, string directoryPath,string fileType ) {
            this.dataSerializeToFormat = dataSerializeToFormat;
            this.directoryPath = directoryPath;
            this.fileType = fileType;
        }
        public async Task<T> GetConservationAsync(string fileName)
        {
            string filePath = Path.Combine(directoryPath, fileName + ".json");

            
            if(!File.Exists(filePath))
            {
                throw new ArgumentException("There exits no Conservation with that name");
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                string conservationJson = await streamReader.ReadToEndAsync();
                T deserializedData = dataSerializeToFormat.DeserializeFromFormat<Message>(conservationJson)
                    ?? throw new Exception("Failed to deserialize JSON.");

                return deserializedData;
            }


        }


        public async Task<List<string>> GetConservationsAsync()
        {
            return await Task.Run(() =>
            {
                string[] files = Directory.GetFiles(directoryPath, fileType);
                string[] fileNames = files.Select(filePath => Path.GetFileName(filePath)).ToArray();

                List<ConservationFilename> conservationFilenames = new List<ConservationFilename>();
                foreach (string file in fileNames)
                {
                    conservationFilenames.Add(new ConservationFilename(file));
                }

                var names = conservationFilenames.OrderByDescending(x => x.ConnverstionHappend).ToList();
                return names.Select(x => x.ToString()).ToList();
            });
        }

      
    }
}
