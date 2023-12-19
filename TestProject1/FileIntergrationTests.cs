using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models;
using TDDD49Lab.Models.Interfaces;

namespace TestProject1
{
    [TestClass]
    public class FileIntergrationTests
    {
        [TestMethod]
        public async Task TestVaildNames()
        {
            // Arange
            string testFilePath = Path.GetTempPath();
            string testFileType = "*.json";
            var DataSeriliaerMock = new Mock<IDataSerialize<ConservationFilename>>();
            HistoryConservation<ConservationFilename> historyConservation = new(DataSeriliaerMock.Object, testFilePath, testFileType);

            // Act
            List<string> filenames = await historyConservation.GetConservationsAsync();
            // Assrt
            Assert.AreEqual(0,filenames.Count);

        }

        [TestMethod]

        public async Task TestOneFileName()
        {
            // Arange
            string testFilePath = Path.GetTempPath();
            string testFileType = "*.json";
            string testFileName = "Henrik-2023-10-29 09-01-57.json";
       
            File.Create(Path.Combine(testFilePath, testFileName)).Close();
            var DataSeriliaerMock = new Mock<IDataSerialize<ConservationFilename>>();

            HistoryConservation<ConservationFilename> historyConservation = new(DataSeriliaerMock.Object, testFilePath, testFileType);
            // Act
            List<string> filenames = await historyConservation.GetConservationsAsync();
            // Assert
            Assert.AreEqual(1, filenames.Count);
            Assert.AreEqual("Henrik-2023-10-29 09-01-57", filenames[0]);

            File.Delete(Path.Combine(testFilePath, testFileName));

        }


        [TestMethod]
        public async Task TestWrongFileType()
        {
            // Arange
            string testFilePath = Path.GetTempPath();
            string testFileType = "*.json";
            string testFileName = "Henrik-2023-10-29 09-01-57.txt";

            File.Create(Path.Combine(testFilePath, testFileName)).Close();
            var DataSeriliaerMock = new Mock<IDataSerialize<ConservationFilename>>();

            HistoryConservation<ConservationFilename> historyConservation = new(DataSeriliaerMock.Object, testFilePath, testFileType);
            // Act
            List<string> filenames = await historyConservation.GetConservationsAsync();
            // Assert
            File.Delete(Path.Combine(testFilePath, testFileName));
            Assert.AreEqual(0, filenames.Count);
      
        }

        [TestMethod]
        public async Task TestOneRightOneFalseFileType()
        {
            // Arange
            string testFilePath = Path.GetTempPath();
            string testFileType = "*.json";
            string testFileNameWrongType = "Wrong-2023-10-29 09-01-57.text";
            string testFileName = "Henrik-2023-10-29 09-01-57.json";

            File.Create(Path.Combine(testFilePath, testFileName)).Close();
            File.Create(Path.Combine(testFilePath, testFileNameWrongType)).Close();
            var DataSeriliaerMock = new Mock<IDataSerialize<ConservationFilename>>();

            HistoryConservation<ConservationFilename> historyConservation = new(DataSeriliaerMock.Object, testFilePath, testFileType);
            // Act
            List<string> filenames = await historyConservation.GetConservationsAsync();
            // Assert

            File.Delete(Path.Combine(testFilePath, testFileName));
            File.Delete(Path.Combine(testFilePath, testFileNameWrongType));
            Assert.AreEqual(1, filenames.Count);
            Assert.AreEqual("Henrik-2023-10-29 09-01-57", filenames[0]);
        }


        [TestMethod]
        public async Task Test5Files()
        {
            // Arange
            string testFilePath = Path.GetTempPath();
            string testFileType = "*.json";
            
            string testFileName1 = "Henrik-2023-10-29 09-01-57.json";
            string testFileName2 = "Henrik-2023-10-29 10-01-57.json";
            string testFileName3 = "Henrik-2023-10-29 11-01-57.json";
            string testFileName4 = "Henrik-2023-10-29 12-01-57.json";
            string testFileName5 = "Henrik-2023-10-29 13-01-57.json";

            File.Create(Path.Combine(testFilePath, testFileName1)).Close();
            File.Create(Path.Combine(testFilePath, testFileName2)).Close();
            File.Create(Path.Combine(testFilePath, testFileName3)).Close();
            File.Create(Path.Combine(testFilePath, testFileName4)).Close();
            File.Create(Path.Combine(testFilePath, testFileName5)).Close();

            var DataSeriliaerMock = new Mock<IDataSerialize<ConservationFilename>>();

            HistoryConservation<ConservationFilename> historyConservation = new(DataSeriliaerMock.Object, testFilePath, testFileType);

            // Act
            List<string> filenames = await historyConservation.GetConservationsAsync();
            // Assert
            File.Delete(Path.Combine(testFilePath, testFileName1));
            File.Delete(Path.Combine(testFilePath, testFileName2));
            File.Delete(Path.Combine(testFilePath, testFileName3));
            File.Delete(Path.Combine(testFilePath, testFileName4));
            File.Delete(Path.Combine(testFilePath, testFileName5));

            Assert.AreEqual(5, filenames.Count);

            Assert.AreEqual("Henrik-2023-10-29 09-01-57", filenames[4]);
            Assert.AreEqual("Henrik-2023-10-29 10-01-57", filenames[3]);
            Assert.AreEqual("Henrik-2023-10-29 11-01-57", filenames[2]);
            Assert.AreEqual("Henrik-2023-10-29 12-01-57", filenames[1]);
            Assert.AreEqual("Henrik-2023-10-29 13-01-57", filenames[0]);
          

        }


        [TestMethod]
        public async Task TestWrongNameFormat()
        {
            // Arange
            string testFilePath = Path.GetTempPath();
            string testFileType = "*.json";
            string testFileName = "Henrik-2023.10.29 09-01-57.json";

            File.Create(Path.Combine(testFilePath, testFileName)).Close();
            var DataSeriliaerMock = new Mock<IDataSerialize<ConservationFilename>>();

            HistoryConservation<ConservationFilename> historyConservation = new(DataSeriliaerMock.Object, testFilePath, testFileType);
            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>  historyConservation.GetConservationsAsync());


            File.Delete(Path.Combine(testFilePath, testFileName));
        }


        [TestMethod]
        public async Task TestWrongFilenamesWrongFileType()
        {
            // Arange
            string testFilePath = Path.GetTempPath();
            string testFileType = "*.json";
            string testFileName = "Henrik-2023.10.29 09-01-57.txt";

            File.Create(Path.Combine(testFilePath, testFileName)).Close();
            var DataSeriliaerMock = new Mock<IDataSerialize<ConservationFilename>>();

            HistoryConservation<ConservationFilename> historyConservation = new(DataSeriliaerMock.Object, testFilePath, testFileType);
            // Act
            List<string> filenames = await historyConservation.GetConservationsAsync();
            // Assert
            Assert.AreEqual(0, filenames.Count);


            File.Delete(Path.Combine(testFilePath, testFileName));
        }


        [TestMethod]
        public async Task TestLoadConservation()
        {

        }


        [TestMethod]
        public async Task TestLoadNotExtingConservation()
        {

        }

        [TestMethod]
        public async Task TestLoadEmptyConservation()
        {

        }
        [TestMethod]
        public async Task TestLoadLongConservation()
        {

        }
        
      


        [TestMethod]
        public void TestLoadInvalidFormat() { }
    }
}
