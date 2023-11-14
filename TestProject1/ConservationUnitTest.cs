
using Moq;
using TDDD49Lab.Models;
using TDDD49Lab.Models.Interfaces;

namespace TestProject1
{
    [TestClass]
    public class ConservationUnitTest
    {

        private Conservation<Message>? _conservation;

        private MemoryStream? _memoryStream;

        [TestInitialize]
        public void TestInitialize()
        {
            // This method will be called before each test method in this class.
            var mockDataSerizliser = new Mock<IDataSerialize<Message>>();
            _memoryStream = new MemoryStream();
            mockDataSerizliser.Setup(x => x.SerializeToFormat(It.IsAny<Message>())).Returns((Message input) => "c");
                
            // Create a new Calculator instance for each test.
            _conservation = new Conservation<Message>(_memoryStream, mockDataSerizliser.Object );
        }


        [TestCleanup]
        public void TestCleanup()
        {
            _memoryStream?.Dispose();
            _conservation = null;
        }



        [TestMethod]
        public async Task TestNotWrittingToStream()
        {
            
            //  Act
            await _conservation.AddMessage(new Message("Henrik", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            //  Assert
           Assert.AreEqual(0, _memoryStream.Length, "The length of the MemoryStream should be zero to indicated that we only write to stream to when we have more the 10 message.");    

        }


        [TestMethod]
        public void TestAddNotItem()
        {

            //  Act
            
            //  Assert
            Assert.AreEqual(0, _memoryStream.Length, "The length of the MemoryStream should be zero to indicated that we only write to stream to when we have more the 10 message.");

        }

        [TestMethod]
        public async Task TestNotWrittingToStream9messages()
        {

            //  Act
            await _conservation.AddMessage(new Message("Henrik1", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik2", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik3", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik4", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik5", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik6", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik7", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik8", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik9", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            //  Assert
            Assert.AreEqual(0, _memoryStream.Length, "The length of the MemoryStream should be zero to indicated that we only write to stream to when we have more the 10 message.");

        }

        [TestMethod]
        public async Task TestNWrittingToStream10messages()
        {

            //  Act
            await _conservation.AddMessage(new Message("Henrik1", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik2", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik3", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik4", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik5", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik6", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik7", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik8", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik9", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik10", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            //  Assert
            Assert.AreEqual(10, _memoryStream.Length, "The length of the MemoryStream should be zero to indicated that we only write to stream to when we have more the 10 message.");

        }


        [TestMethod]
        public async Task TestNWrittingToStream11messages()
        {

            //  Act
            await _conservation.AddMessage(new Message("Henrik1", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik2", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik3", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik4", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik5", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik6", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik7", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik8", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik9", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik10", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            await _conservation.AddMessage(new Message("Henrik11", "Hello World", true, DateTime.Now, NetworkProtocolTypes.Message));
            //  Assert
            Assert.AreEqual(10, _memoryStream.Length, "The length of the MemoryStream should be zero to indicated that we only write to stream to when we have more the 10 message.");

        }

    }
}
