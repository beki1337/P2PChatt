using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    internal class HistoryConservation<T> : IHistoryConservation<T>
    {


        private DataSerializeToFormat<T> dataSerializeToFormat;
        public HistoryConservation(DataSerializeToFormat<T> dataSerializeToFormat) {
            this.dataSerializeToFormat = dataSerializeToFormat;
        }
        public Task<IEnumerable<T>> GetConservationAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetConservationsAsync()
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

                return conservationFilenames.OrderByDescending(x => x.ConnverstionHappend).ToList();
            });
        }
    }
}
