using Moq;
using TDDD49Lab;
using TDDD49Lab.Models;
using TDDD49Lab.Models.Interfaces;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            Chatt chatt = new Chatt();
            var mockITcpClient = new Mock<ITcpClient>();
            mockITcpClient.Setup(x => x.GetStream()).Returns(new MemoryStream());
            
            // Act
            // Assert

        }
    }
}