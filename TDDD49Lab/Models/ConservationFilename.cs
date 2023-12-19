using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TDDD49Lab.Models
{
    public readonly struct ConservationFilename
    {

        private readonly string pattern = @"(?<Username>[^\d]+)-(?<Date>\d{4}-\d{2}-\d{2} \d{2}-\d{2}-\d{2})";

        public string User { get; init; }
        public DateTime ConnverstionHappend { get; init; }
        public ConservationFilename(string filename) {

            filename = filename.Substring(0, filename.Length - 5);
            Match match = Regex.Match(filename, pattern);

            if (!match.Success)
            {
                Debug.WriteLine(filename);
                var dd = match.Groups["Username"].Value;
                Debug.WriteLine(dd);
                throw new ArgumentException(filename);
            }

            User = match.Groups["Username"].Value;  
            
            if (DateTime.TryParseExact(match.Groups["Date"].Value, "yyyy-MM-dd HH-mm-ss", null, DateTimeStyles.None, out DateTime result))
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




   
        public override string ToString()
        {
            return $"{User}-{ConnverstionHappend.ToString("yyyy-MM-dd HH-mm-ss")}";
        }
    }
}
