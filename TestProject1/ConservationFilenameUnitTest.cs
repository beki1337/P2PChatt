using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models;

namespace TestProject1
{
    [TestClass]
    public class ConservationFilenameUnitTest
    {

        [TestMethod]
        [DataRow("Hentris-2023-10-29 09-01-57", "Hentris-2023-10-29 09-01-57")]
        [DataRow("H-2023-10-29 09-01-57", "H-2023-10-29 09-01-57")]
        [DataRow("h-2023-10-29 09-01-57", "h-2023-10-29 09-01-57")]
        [DataRow("Henrik-0001-01-01 00-00-00", "Henrik-0001-01-01 00-00-00")]
        [DataRow("Henrik-9999-12-31 23-59-59", "Henrik-9999-12-31 23-59-59")]
        [DataRow("henrik-2023-10-29 09-01-57", "henrik-2023-10-29 09-01-57")]
        [DataRow("HENRIK-2023-10-29 09-01-57", "HENRIK-2023-10-29 09-01-57")]
        public void TestVaildFilenames(string filename,string expectedFilename)
        {
            ConservationFilename conservationFilename = new ConservationFilename(filename);
            Assert.AreEqual(expectedFilename, conservationFilename.ToString());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Henrik")]
        [DataRow("2023-10-29 09-01-57")]
        [DataRow("Henrik-2-2023-10-29 09-01-57")]
        [DataRow("Henrik-2023:10:29-09:01:57")]
        [DataRow("Henrik-09-01-57 2023-10-29")]
        [DataRow("Henrik-2023-10-29 9-1-57")]
        public void TestNotVaildFilenames(string filename) {
            // Arrange & Act
            Action act = () => new ConservationFilename(filename);

            // Assert
            Assert.ThrowsException<ArgumentException>(act, "The file does not have a valid format");
        }

    }
}
