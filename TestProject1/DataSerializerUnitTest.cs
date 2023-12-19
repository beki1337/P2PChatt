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
    public class DataSerializerUnitTest
    {

        private static DataSerializer<Message> serializer = new DataSerializer<Message>();


    


        [TestMethod]
        public void TestSerilaObjext()
        {
            var message = new Message("Username", "Hello world", true,new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.Message); ;
            string json = serializer.SerializeToFormat(message);
        }

        [TestMethod]
        public void TestSerillaNoName()
        {
            var message = new Message("", "Hello world", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.Message);
            string json = serializer.SerializeToFormat(message);
        }

        [TestMethod]
        public void TestNoMessageNoName()
        {
            var message = new Message("", "", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.Message);
            string json = serializer.SerializeToFormat(message);
        }

        [TestMethod]
        public void TestNoMessage()
        {
            var message = new Message("Henrik", "", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.Message);
            string json = serializer.SerializeToFormat(message);
        }



            [TestMethod]
        public void TestSerilaRightNetworkPortoclType() {
            var message = new Message("Username", "Helloworld", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.Message);
            string json = serializer.SerializeToFormat(message);

           
        }

        [TestMethod]
        public void TestSerilRightNightProctcolEstablishConnection()
        {
            var message = new Message("Username", "Helloworld", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.EstablishConnection);
            string json = serializer.SerializeToFormat(message);    
        }

        [TestMethod]
        public void TestSerilaRigthNetworkroktokBuzz()
        {
            var message = new Message("Username", "Helloworld", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.Buzz);
            string json = serializer.SerializeToFormat(message);
        }

        [TestMethod]
        public void TestSerilaizerRightNetworkTypeDisconnect()
        {
            var message = new Message("Username", "Helloworld", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.Disconnect);
            string json = serializer.SerializeToFormat(message);
        }


        [TestMethod]
        public void TestSerilaizerRightNetworkDeniedConnection()
        {
            var message = new Message("Username", "Helloworld", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.DeniedConnection);
            string json = serializer.SerializeToFormat(message);
        }

        [TestMethod]
        public void TestSerilaizerNetworkAcceptConnection()
        {
            var message = new Message("Username", "Helloworld", true, new DateTime(2008, 5, 1, 8, 30, 52), NetworkProtocolTypes.AcceptConnection);
            string json = serializer.SerializeToFormat(message);
        }


        [TestMethod]
        public void  TestSerilaizerDateLowest()
        {
            var message = new Message("Username", "Hello world", true, DateTime.MinValue, NetworkProtocolTypes.Message);
        }


        [TestMethod]
        public void TestSerilaizerDateLagerst()
        {
            var message = new Message("Username", "Hello world", true, DateTime.MaxValue, NetworkProtocolTypes.Message);
        }

    }
}
