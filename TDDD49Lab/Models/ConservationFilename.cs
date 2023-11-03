using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models
{
    public readonly struct ConservationFilename
    {


        public ConservationFilename(string filename) {
            int datetimeLenght = 19;
            string cleanfilename = filename.Substring(0, filename.Length - 5);
            string username = cleanfilename.Substring(0, cleanfilename.Length - 20);
            User = username;
            string datetimeString = cleanfilename.Substring(cleanfilename.Length - datetimeLenght);
            if (DateTime.TryParseExact(datetimeString, "yyyy-MM-dd HH-mm-ss", null, DateTimeStyles.None, out DateTime result))
            {
                // Parsing successful, 'result' now contains the DateTime object.
                ConnverstionHappend = result;
            }
            else
            {
                // Parsing failed, handle the invalid input as needed.
                throw new ArgumentException("The file dose is not vailf");
            }
        }
   
        public string User { get; init; }

        public DateTime ConnverstionHappend { get; init; }


        public override string ToString()
        {
            return $"{User}-{ConnverstionHappend.ToString("yyyy-MM-dd HH-mm-ss")}";
        }
    }
}
