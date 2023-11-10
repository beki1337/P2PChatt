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

namespace TDDD49Lab.Models
{
    public  class Chatt : IChatt
    {
        private ITcpClient? _tcpClient;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private string _username = "Unkown";
        private string _otheruserName = "Unkown";
        private bool _isRunning = false;
        private bool _isConnected = false;
        private Conservation conservation = new Conservation("Hentris");

        public delegate bool ShowMessageBoxDelegate(string username);

      
        public async Task Serach(ShowMessageBoxDelegate showMessageBox,ITcpClient tcpClient)
        {
            if (!_isRunning || !_isConnected)
            {
                throw new InvalidOperationException("The method cannot be executed because the chatt is running or connected.");
            }
            _tcpClient = tcpClient;
            var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 13);
          
            try
            {
                _isRunning = true;
                await _tcpClient.ConnectAsync(ipEndPoint);
                _reader = new StreamReader(_tcpClient.GetStream());
                _writer = new StreamWriter(_tcpClient.GetStream());
               
                await SendConnectionRequest("Serachin");
                string otherUserName = await _reader.ReadLineAsync() ?? "null";
                await AskUserToAcceptConnectionAsync(otherUserName,  showMessageBox);
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().Name);
            }
            await ListenForMessages();

        }


        public async Task StartListingForUsers(ShowMessageBoxDelegate showMessageBox, ITcpListener tcpListener)
        {
            if (!_isRunning || !_isConnected)
            {
                throw new InvalidOperationException("The method cannot be executed because the chatt is running or connected.");
            }
            _username = "Listing";
            _isRunning = true;
            ITcpListener listener = tcpListener;

            listener.Start();
            ITcpClient _tcpClient = await listener.AcceptTcpClientAsync();
            listener.Stop();
            _reader = new StreamReader(_tcpClient.GetStream());
            _writer = new StreamWriter(_tcpClient.GetStream());
            await SendConnectionRequest("Listener");
            string otherUserName = await _reader.ReadLineAsync() ?? "null";
            await AskUserToAcceptConnectionAsync(otherUserName, showMessageBox);
            await ListenForMessages();
        }


       
        private async Task AskUserToAcceptConnectionAsync(string otherUserName, ShowMessageBoxDelegate showMessageBox)
        {
            NetWorkProtocolChatt netWorkProtocolChatt = NetWorkProtocolChatt.FromJson(otherUserName);
            
            if (!showMessageBox(netWorkProtocolChatt.Username))
            {
                if (_writer != null)
                {
                    await SendDeclineRequest(otherUserName);
                }
                else
                {
                    throw new NullReferenceException("The _write is null");
                }

            }
        }


        public bool IsRunning()
        {
            return _isRunning;
        }


        public void SubscribeToMessage(MessageRecivedEventHandler method)
        {
            MessageReceived += method;
        }


        public async Task Disconnect()
        {

            await SendDisconnectMessage("Defulat");
            _isRunning = false;
            CloseClients();
        }


        private event MessageRecivedEventHandler MessageReceived;


        public delegate void MessageRecivedEventHandler(object sender, MessageEventArgs e);

        private string MakeConnenectionMessage(string username)
        {
            return new NetWorkProtocolChatt(NetworkProtocolTypes.EstablishConnection, username,"").ToJson();
        }

        private string CreateDeclineConection(string username)
        {
            return new NetWorkProtocolChatt(NetworkProtocolTypes.DeniedConnection, username, "").ToJson();
        }

        private string CreatDisconnectMessage(string username)
        {
            return new NetWorkProtocolChatt(NetworkProtocolTypes.Disconnect, username, "").ToJson();
        }

        private string CreatMesssage(string username,string message)
        {
            return new NetWorkProtocolChatt(NetworkProtocolTypes.Message, username, message).ToJson();
        }


        private async Task SendConnectionRequest(string username)
        {
            string message = MakeConnenectionMessage(username);
            await SendMessage(message);
           
        }


        public async Task SendUserMessage(string username, string message)
        {
            string message1 = CreatMesssage(username, message);
            conservation.AddMessage(new("asdasd","asdfsdfdsfsdfdsf",true,DateTime.Now));
            await SendMessage(message1);
            OnMessageRecived(username, message);
        }

        private async Task SendDeclineRequest(string username)
        {
            string message = CreateDeclineConection(username);
            await SendMessage(message);
            _isRunning = false;
            CloseClients();
        }

        private async Task SendDisconnectMessage(string username)
        {
            string message = CreatDisconnectMessage(username);
            await SendMessage(message);
            _isRunning = false;
            CloseClients();
            await conservation.SendToWritte();
        }



        private void CloseClients()
        {
            _reader?.Close();
            _writer?.Close();
        }

        public async Task SendMessage(string message)
        {
            message+="\n";
           
            if (_writer is not null)
            {
                await _writer.WriteAsync(message);
                await _writer.FlushAsync();
            }

        }


        private void OnMessageRecived(string username, string message)
        {
            MessageReceived?.Invoke(this, new MessageEventArgs(username, message));
        }

        private async Task ListenForMessages()
        {

           

            while (IsRunning())
            {
                await Task.Delay(5000);
                
                string receivedData = await _reader.ReadLineAsync() ?? throw new ArgumentNullException("Haha");
              
                
                NetWorkProtocolChatt netWorkProtocolChatt = NetWorkProtocolChatt.FromJson(receivedData);
                switch (netWorkProtocolChatt.Type)
                {
                    case "deniedconnection":
                        MessageBox.Show($"The other part diend the request {receivedData}");
                        CloseClients();
                        _isRunning = false;
                        break;
                    case "disconnect":
                        MessageBox.Show($"The other part has shose to disconect {receivedData}");
                        CloseClients();
                        await conservation.SendToWritte();
                        _isRunning = false;
                        break;
                    case "message":
                        MessageBox.Show($"Recived message {netWorkProtocolChatt.Message}");
                        OnMessageRecived(_username, netWorkProtocolChatt.Message);
                        conservation.AddMessage(new(netWorkProtocolChatt.Username, netWorkProtocolChatt.Message, false,DateTime.Now));
                        break;
                    default:
                        break;
                }

            }


            MessageBox.Show("We have ende a cylce");
        }




    }
}
