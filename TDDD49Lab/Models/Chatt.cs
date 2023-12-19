using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TDDD49Lab.Models.ClassEventArgs;
using TDDD49Lab.Models.Interfaces;
using TDDD49Lab.Models.Enums;

namespace TDDD49Lab.Models
{
    public class Chatt : IChatt
    {

        private readonly INetworkHandler<IMessage> _networkHandler;
        private readonly IConservation<IMessage> _conservation;

        private string sdsd;
        

        public Chatt(INetworkHandler<IMessage> handler, IConservation<IMessage> conservation)
        {
            
            _networkHandler = handler;
            _conservation = conservation;
        }




        public async Task<string> SearchAsync(string username, string ipAddres, int port)
        {
            sdsd = "serache";
            IMessage retrivedUsername = await _networkHandler.Connect(ipAddres, port);
            return retrivedUsername.Username;
        }


        public async Task<string> StartListingForUsersAsync(string username, string ipAddres, int port)
        {
            sdsd = "Listeing";
            IMessage message = await _networkHandler.Listen(ipAddres, port);
            return message.Username;
        }



        public bool IsRunning()
        {
            return _networkHandler.CurrentNetworkStatet() != NetworkState.Ideal;
        }


        public void SubscribeToMessage(MessageRecivedEventHandler method)
        {
            MessageReceived += method;
        }

        public async Task DisconnectAsync()
        {

            await _networkHandler.Disconect();
            DisconnectReceived?.Invoke(this);
        }

        public async Task DeclineRequest()
        {
            await _networkHandler.DeclineRequest();
        }

        public async Task<bool> AcceptRequest()
        {
           return  await _networkHandler.AcceptConnection();
        }


        public delegate void MessageRecivedEventHandler(object sender, MessageEventArgs e);

        public delegate void DisconnecRecivedEventHandler(object sender);

        private event MessageRecivedEventHandler MessageReceived;

        private event DisconnecRecivedEventHandler DisconnectReceived;
        public async Task SendMessageAsync(string message)
        {
            await _networkHandler.SendMessage(message);
        }



        private void OnMessageRecived(string username, string message)
        { 
           MessageReceived?.Invoke(this, new MessageEventArgs(username, message));
        }

       

        public async Task ListenForMessagesAsync()
        {
            bool ended = false;
            await foreach (IMessage message in _networkHandler.GetMessages())
            {

                if (ended)
                {
                    break;
                }
                switch (message.NetworkProtocolType)
                {
                    case NetworkProtocolTypes.Message:
                        OnMessageRecived(message.Username, message.MessageText);
                        await _conservation.AddMessage(message);
                        break;
                    case NetworkProtocolTypes.Buzz:
                        break;
                    case NetworkProtocolTypes.Disconnect:
                        MessageBox.Show("It has been disconnect");
                        _networkHandler.Dispose();
                        DisconnectReceived?.Invoke(this);
                        ended = true;
                        break;
                    default:
                        MessageBox.Show("default"); break;

                }
            }
            MessageBox.Show($"NU är vi klara med den här saken {sdsd}");

        }

        public void SubscribeToDisconnec(DisconnecRecivedEventHandler method)
        {
            DisconnectReceived += method;
        }
    }
    
}
