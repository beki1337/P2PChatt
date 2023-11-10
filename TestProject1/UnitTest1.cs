using Moq;
using System.Net;
using System.Text;
using System.Windows.Documents;
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

            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            mockINetworkProtocol.Setup(x => x.DecodeStringeAsync(It.IsAny<string>())).ReturnsAsync((string input) => input);
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

            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient,mockINetworkProtocol.Object);


            // Act
            string username = await networkHandler.Connect("127.0.0.1", 80);

            // Assert
            Assert.AreEqual(username, testData);

        }

        [TestMethod]
        public async Task TestWrongFormat()
        {
            // Arrange
            var mockINetworkProtocol = new Mock<INetworkProtocol>();
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

            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);


            // Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                string username = await networkHandler.Connect("Invalid IP address format.", 80);
            });

        }

        [TestMethod]
        public async Task ReciviedNullUsername()
        {
            // Arrange

            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            var mockITcoListner = new Mock<ITcpListener>();

            var mockITcpClient = new Mock<ITcpClient>();
            string testData = string.Empty;
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(new MemoryStream(testBytes));
            mockITcpClient
            .Setup(x => x.ConnectAsync(It.IsAny<IPEndPoint>()))
            .Returns(Task.CompletedTask);

            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockITcoListner.Object;

            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);


            // Act && Assert

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                string username = await networkHandler.Connect("127.0.0.1", 80);
            });
        }



        [TestMethod]
        public async Task StartListningen()
        {
            // Arrange
            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            mockINetworkProtocol.Setup(x => x.DecodeStringeAsync(It.IsAny<string>())).ReturnsAsync((string input) => input);
            var mockTcpListner = new Mock<ITcpListener>();
            var mockITcpClient = new Mock<ITcpClient>();
            mockTcpListner.Setup(x => x.AcceptTcpClientAsync())
                .ReturnsAsync(mockITcpClient.Object);

            mockTcpListner.Setup(x => x.Start());
            mockTcpListner.Setup(x => x.Stop());    
            string testData = "This is a test string.";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(new MemoryStream(testBytes));
            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockTcpListner.Object;
            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);
            // Act
            string username = await networkHandler.Listen("127.0.0.1", 80);

            // Assert

            Assert.AreEqual(username, testData);
        }


        [TestMethod]
        public async Task TestReadMessageNotCorrectState()
        {
            // Arrange
            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            mockINetworkProtocol.Setup(x => x.DecodeStringeAsync(It.IsAny<string>())).ReturnsAsync((string input) => input);
            var mockTcpListner = new Mock<ITcpListener>();
            var mockITcpClient = new Mock<ITcpClient>();
            mockTcpListner.Setup(x => x.AcceptTcpClientAsync())
                .ReturnsAsync(mockITcpClient.Object);

            mockTcpListner.Setup(x => x.Start());
            mockTcpListner.Setup(x => x.Stop());
            string testData = "This is a test string.\n Here gose the other string.\n";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(new MemoryStream(testBytes));
            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockTcpListner.Object;
            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);
            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => {
                await using var enumerator = networkHandler.GetMessages().GetAsyncEnumerator();
                await enumerator.MoveNextAsync();
            });

        }


        [TestMethod]
        public async Task TestReadMessageCorrectState()
        {
            // Arrange
            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            mockINetworkProtocol.Setup(x => x.DecodeStringeAsync(It.IsAny<string>())).ReturnsAsync((string input) => input);
            var mockTcpListner = new Mock<ITcpListener>();
            var mockITcpClient = new Mock<ITcpClient>();
            mockTcpListner.Setup(x => x.AcceptTcpClientAsync())
                .ReturnsAsync(mockITcpClient.Object);

            mockTcpListner.Setup(x => x.Start());
            mockTcpListner.Setup(x => x.Stop());
            string testData = "This is a test string.\nHere gose the other string.\n";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(new MemoryStream(testBytes));
            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockTcpListner.Object;
            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);
            
            // Act
            string username = await networkHandler.Connect("127.0.0.1", 80);
            networkHandler.AccepetConnection();
            await using var enumerator = networkHandler.GetMessages().GetAsyncEnumerator();
            await enumerator.MoveNextAsync();
            // Assert

            string? message = enumerator.Current;
            Assert.AreEqual("Here gose the other string.", message);

        }


        [TestMethod]
        public async Task TestReadMessage()
        {
            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            mockINetworkProtocol.Setup(x => x.DecodeStringeAsync(It.IsAny<string>())).ReturnsAsync((string input) => input);
            var mockTcpListner = new Mock<ITcpListener>();
            var mockITcpClient = new Mock<ITcpClient>();
            mockTcpListner.Setup(x => x.AcceptTcpClientAsync())
                .ReturnsAsync(mockITcpClient.Object);

            mockTcpListner.Setup(x => x.Start());
            mockTcpListner.Setup(x => x.Stop());
            string testData = "This is a test string.\n";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            MemoryStream memoryStream = new MemoryStream(testBytes);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(memoryStream);
            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockTcpListner.Object;
            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);

            // Act
            string username = await networkHandler.Connect("127.0.0.1", 80);
            networkHandler.AccepetConnection();
            await using var enumerator = networkHandler.GetMessages().GetAsyncEnumerator();
            await enumerator.MoveNextAsync();
            // Assert

            string? message = enumerator.Current;
            Assert.AreEqual(null, message);
        }

        [TestMethod]
        public async Task SendMessage()
        {
            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            mockINetworkProtocol.Setup(x => x.DecodeStringeAsync(It.IsAny<string>())).ReturnsAsync((string input) => "input");
            mockINetworkProtocol.Setup(x => x.CreateMessageAsync(It.IsAny<string>())).ReturnsAsync((string input) => input);
            var mockTcpListner = new Mock<ITcpListener>();
            var mockITcpClient = new Mock<ITcpClient>();
            mockTcpListner.Setup(x => x.AcceptTcpClientAsync())
                .ReturnsAsync(mockITcpClient.Object);

            mockTcpListner.Setup(x => x.Start());
            mockTcpListner.Setup(x => x.Stop());
            string testData = "This is a test string.\n Hello world\n";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            MemoryStream memoryStream = new MemoryStream(1020);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(memoryStream);
            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockTcpListner.Object;
            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);
            string messageToSend = "Hello world";

            // Act
            string username = await networkHandler.Connect("127.0.0.1", 80);
            networkHandler.AccepetConnection();
            await networkHandler.SendMessage(messageToSend);

            // Assert
            memoryStream.Position = 0;
            using StreamReader reader = new StreamReader(memoryStream);
            string? contentOnStream = await reader.ReadLineAsync();
            Assert.AreEqual(messageToSend, contentOnStream);


        }


        [TestMethod]
        public async Task SendMessageNTimes()
        {
            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            mockINetworkProtocol.Setup(x => x.DecodeStringeAsync(It.IsAny<string>())).ReturnsAsync((string input) => "input");
            mockINetworkProtocol.Setup(x => x.CreateMessageAsync(It.IsAny<string>())).ReturnsAsync((string input) => input);
            var mockTcpListner = new Mock<ITcpListener>();
            var mockITcpClient = new Mock<ITcpClient>();
            mockTcpListner.Setup(x => x.AcceptTcpClientAsync())
                .ReturnsAsync(mockITcpClient.Object);

            mockTcpListner.Setup(x => x.Start());
            mockTcpListner.Setup(x => x.Stop());
            string testData = "This is a test string.\n Hello world\n";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            MemoryStream memoryStream = new MemoryStream(1020);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(memoryStream);
            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockTcpListner.Object;
            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);
            string messageToSend = "1\n2\n3\n4\n";

            // Act
            string username = await networkHandler.Connect("127.0.0.1", 80);
            networkHandler.AccepetConnection();
            await networkHandler.SendMessage(messageToSend);

            // Assert
            memoryStream.Position = 0;
            using StreamReader reader = new StreamReader(memoryStream);
            string? contentOnStream = await reader.ReadLineAsync();
            Assert.AreEqual("1", contentOnStream);

           
            string? contentOnStream2 = await reader.ReadLineAsync();
            Assert.AreEqual("2", contentOnStream2);

           
            string? contentOnStream3 = await reader.ReadLineAsync();
            Assert.AreEqual("3", contentOnStream3);

            string? contentOnStream4 = await reader.ReadLineAsync();
            Assert.AreEqual("4", contentOnStream4);

            string? contentOnStreamNull = await reader.ReadLineAsync();
            Assert.IsNull(contentOnStreamNull);


        }

        [TestMethod]
        public async Task SendMessageEmptyMessage()
        {
            var mockINetworkProtocol = new Mock<INetworkProtocol>();
            mockINetworkProtocol.Setup(x => x.DecodeStringeAsync(It.IsAny<string>())).ReturnsAsync((string input) => "input");
            mockINetworkProtocol.Setup(x => x.CreateMessageAsync(It.IsAny<string>())).ReturnsAsync((string input) => input);
            var mockTcpListner = new Mock<ITcpListener>();
            var mockITcpClient = new Mock<ITcpClient>();
            mockTcpListner.Setup(x => x.AcceptTcpClientAsync())
                .ReturnsAsync(mockITcpClient.Object);

            mockTcpListner.Setup(x => x.Start());
            mockTcpListner.Setup(x => x.Stop());
            string testData = "This is a test string.\n Hello world\n";
            byte[] testBytes = Encoding.UTF8.GetBytes(testData);
            MemoryStream memoryStream = new MemoryStream(1020);
            mockITcpClient
                .Setup(x => x.GetStream())
                .Returns(memoryStream);
            Func<ITcpClient> createClient = () => mockITcpClient.Object;
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint enPoint) => mockTcpListner.Object;
            NetworkHandler networkHandler = new NetworkHandler(createListner, createClient, mockINetworkProtocol.Object);
            string messageToSend = "";

            // Act
            string username = await networkHandler.Connect("127.0.0.1", 80);
            networkHandler.AccepetConnection();
            await networkHandler.SendMessage(messageToSend);

            // Assert
            memoryStream.Position = 0;
            using StreamReader reader = new StreamReader(memoryStream);
            string? contentOnStream = await reader.ReadLineAsync();
            Assert.IsNull(contentOnStream);


        }


    }
}