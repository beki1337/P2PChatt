using Moq;
using System.Net;
using System.Text;
using TDDD49Lab;
using TDDD49Lab.Models;
using TDDD49Lab.Models.Interfaces;
using TDDD49Lab.ViewModels;
using static TDDD49Lab.Models.Chatt;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var mockITcoListner = new Mock<ITcpListener>();

            var mockITcpClient = new Mock<ITcpClient>();
            string testData = "This is a test string.";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(new MemoryStream(testBytes));
            mockITcpClient
            .Setup(x => x.ConnectAsync(It.IsAny<IPEndPoint>()))
            .Returns(Task.CompletedTask);

            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockITcoListner.Object;

            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient);


            // Act
            string username = await networkHandler.Connect("127.0.0.1", 80);

            // Assert
            Assert.AreEqual(username, testData);

        }

        [TestMethod]
        public async Task TestWrongFormat()
        {
            // Arrange
            var mockITcoListner = new Mock<ITcpListener>();

            var mockITcpClient = new Mock<ITcpClient>();
            string testData = "This is a test string.";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(new MemoryStream(testBytes));
            mockITcpClient
            .Setup(x => x.ConnectAsync(It.IsAny<IPEndPoint>()))
            .Returns(Task.CompletedTask);

            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockITcoListner.Object;

            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient);


            // Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                string username = await networkHandler.Connect("Invalid IP address format.", 80);
            });

        }
        

        public async Task ReciviedNullUsername()
        {
            // Arrange
            var mockITcoListner = new Mock<ITcpListener>();

            var mockITcpClient = new Mock<ITcpClient>();
            string testData = "This is a test string.";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(new MemoryStream(testBytes));
            mockITcpClient
            .Setup(x => x.ConnectAsync(It.IsAny<IPEndPoint>()))
            .Returns(Task.CompletedTask);

            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockITcoListner.Object;

            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient);


            // Act && Assert

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                string username = await networkHandler.Connect("127.0.0.1", 80);
            });
        }

    }
}