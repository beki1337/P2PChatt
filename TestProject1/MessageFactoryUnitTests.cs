using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models;
using TDDD49Lab.Models.Interfaces;

namespace TestProject1
{
    [TestClass]
    public class MessageFactoryUnitTests
    {

        private static MessageFactory _factory;

        [ClassInitialize]
        public static void ClassInitialize(TestContext textContext)
        {
            var mockIDateObject = new Mock<IDateTime>();
            mockIDateObject.Setup(x => x.Now).Returns(new DateTime(2008,09,01));
            _factory = new MessageFactory(mockIDateObject.Object);
        }


        [TestMethod]
        public void TestCreateMessage()
        {
            // Act
            IMessage message = _factory.CreateMessage("Henrik", "Hello World");


            // Assert
            Assert.AreEqual("Henrik", message.Username);
            Assert.AreEqual("Hello World", message.MessageText);
            Assert.AreEqual(new DateTime(2008, 09, 01), message.DateTime);
            Assert.IsTrue(message.IsYourMessage);
            Assert.AreEqual(NetworkProtocolTypes.Message, message.NetworkProtocolType);
        }

        [TestMethod]
        public void TestCreateConnectionHandshake()
        {
            // Act
            
            IMessage message = _factory.CreateConnectionHandshake("Henrik");
            // Assert
            Assert.AreEqual("Henrik", message.Username);
            Assert.AreEqual("", message.MessageText);
            Assert.AreEqual(new DateTime(2008,09,01), message.DateTime);
            Assert.IsTrue(message.IsYourMessage);
            Assert.AreEqual(NetworkProtocolTypes.EstablishConnection, message.NetworkProtocolType);
        }


        [TestMethod]
        public void TestCreateDeclineHandshake() {
            // Act

            IMessage message = _factory.CreateDeclineHandshake("Henrik");
            // Assert
            Assert.AreEqual("Henrik", message.Username);
            Assert.AreEqual("", message.MessageText);
            Assert.AreEqual(new DateTime(2008, 09, 01), message.DateTime);
            Assert.IsTrue(message.IsYourMessage);
            Assert.AreEqual(NetworkProtocolTypes.DeniedConnection, message.NetworkProtocolType);
        }

        [TestMethod]
        public void TestCreateAcceptHandshake() {
            // Act

            IMessage message = _factory.CreateAcceptHandshake("Henrik");
            // Assert
            Assert.AreEqual("Henrik", message.Username);
            Assert.AreEqual("", message.MessageText);
            Assert.AreEqual(new DateTime(2008, 09, 01), message.DateTime);
            Assert.IsTrue(message.IsYourMessage);
            Assert.AreEqual(NetworkProtocolTypes.AcceptConnection, message.NetworkProtocolType);
        }

        [TestMethod]
        public void CreateDisconnect()
        {
            // Act
            IMessage message = _factory.CreateDisconnect("Henrik");
            // Assert
            Assert.AreEqual("Henrik", message.Username);
            Assert.AreEqual("", message.MessageText);
            Assert.AreEqual(new DateTime(2008, 09, 01), message.DateTime);
            Assert.IsTrue(message.IsYourMessage);
            Assert.AreEqual(NetworkProtocolTypes.Disconnect, message.NetworkProtocolType);
        }


        [TestMethod]
        public void TestCreateBuzz() {
            // Act
            IMessage message = _factory.CreateDisconnect("Henrik");
            // Assert
            Assert.AreEqual("Henrik", message.Username);
            Assert.AreEqual("", message.MessageText);
            Assert.AreEqual(new DateTime(2008, 09, 01), message.DateTime);
            Assert.IsTrue(message.IsYourMessage);
            Assert.AreEqual(NetworkProtocolTypes.Disconnect, message.NetworkProtocolType);
        }

       

    }
}
