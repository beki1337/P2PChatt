using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDDD49Lab.Models;
using TDDD49Lab.Models.ClassEventArgs;
using TDDD49Lab.Models.Interfaces;
using TDDD49Lab.Models.WrapperModels;

namespace TDDD49Lab.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IChatt _chatt;
        private ICommand _listenForConnections;
        private ICommand _searchForConnection;
        private ICommand _disconnect;
        private ICommand _sendMessage;


        private string message = string.Empty;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }


        private string username = string.Empty;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }

        }


        private string ipAddres = string.Empty;

        public string IpAddres
        {
            get { return ipAddres; }
            set
            {
                ipAddres = value;
                OnPropertyChanged(nameof(IpAddres));
            }
        }

        private int port = 0;
        public int Port
        {
            get { return port; }
            set {
                port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        private ConservationFilename selected_chatt;
        public ConservationFilename SelectedPerson
        {
            get
            { 
                return selected_chatt;
            }
            set
            {
                
                _items = new ChattHistory().GetMessages(value.ToString());
                selected_chatt = value;
                OnPropertyChanged(nameof(Messages));
                _chatPartner = value.ToString();
                OnPropertyChanged(nameof(ChatPartner));
            }
        }


        private bool canSeeChattHistory = true;
        public bool CanSeeChatthistory { get { return canSeeChattHistory; } }

        private ObservableCollection<Message> _items;


        public string _chatPartner = string.Empty;

        public string ChatPartner { get { return _chatPartner; }  }
        public ObservableCollection<Message> Messages
        {
            get { return _items; }
            
        }

        public List<ConservationFilename> Conservations
        {
            get {
                var ss = new ChattHistory().GetConneversations();
                return ss;
            }
        }

        public ICommand ListenForConnections
        {
            get { return _listenForConnections; }
        }

        public ICommand SearchForConnection
        {
            get { return _searchForConnection; }
            
        }

        public ICommand Disconnect
        {
            get { return _disconnect; }
          
        }
        public ICommand SendMessage
        {
            get { return _sendMessage; }
        }

        public MainViewModel(IChatt chatt) {
            _chatt = chatt;
            _searchForConnection =  new AsyncRelayCommand(SearchForUser, CanMakeNewConnection, (ex) => MessageBox.Show($" Eroror from Seacrhing {ex.Message} {ex.StackTrace}"));
            _listenForConnections = new AsyncRelayCommand(StartListningForUsers, CanMakeNewConnection, (ex) => MessageBox.Show($"Error from Lsiting {ex.Message} {ex.StackTrace}"));
            _disconnect = new AsyncRelayCommand(Disconect, IsConnected, (ex) => MessageBox.Show(ex.Message));
            _sendMessage = new AsyncRelayCommand(SendUserMessage, IsConnected, (ex) => MessageBox.Show($"Error when sending message {ex.Message} {ex.StackTrace}"));
            _items = new ObservableCollection<Message>();
            _chatt.SubscribeToMessage(IChatt_MessageReviced);
            _chatt.SubscribeToDisconnec(IChattDisconnect);





            CommandManager.RequerySuggested += (sender, e) =>
            {
                ((AsyncRelayCommand)_searchForConnection).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)_listenForConnections).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)_disconnect).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)_sendMessage).RaiseCanExecuteChanged();
             
            };

        }

        private async Task Disconect(object obj)
        {
            SystemSounds.Beep.Play();
            await _chatt.DisconnectAsync();
            canSeeChattHistory = true;
            OnPropertyChanged(nameof(CanSeeChatthistory));
        }

        private async Task StartListningForUsers(object obj)
        {            
            canSeeChattHistory = false;
            OnPropertyChanged(nameof(CanSeeChatthistory));
            string conecthinguser = await _chatt.StartListingForUsersAsync(username, ipAddres, port);
            if (ShowMessageBox(conecthinguser))
            {
                if(!await _chatt.AcceptRequest())
                {
                    MessageBox.Show("The other user denied the connection");
                    //IChattDisconnect(this);
                } else
                {
                    MessageBox.Show("Hur fan kan vi hammna här ?");
                    await _chatt.ListenForMessagesAsync();
                }
                
            } else
            {
                await _chatt.DeclineRequest();
                IChattDisconnect(this);
            }
        }

        private bool CanMakeNewConnection(object obj)
        {
            return !_chatt.IsRunning();
        }

        private bool IsConnected(object obj)
        {
            return _chatt.IsRunning();
        }


        private async Task SearchForUser(object obj)
        {
            canSeeChattHistory = false;
            OnPropertyChanged(nameof(CanSeeChatthistory));
            string listingUsername = await _chatt.SearchAsync(username,ipAddres,port);
            if (ShowMessageBox(listingUsername))
            {
                if (!await _chatt.AcceptRequest())
                {
                    MessageBox.Show("The other user denied the connection");
                    //await _chatt.ListenForMessagesAsync();
                } else
                {
                    await _chatt.ListenForMessagesAsync();
                }
                
            }
            else
            {
                await _chatt.DeclineRequest();
                IChattDisconnect(this);
            }


           
        }


        private async Task SendUserMessage(object obj)
        {
            _items.Add(new Message("Slef", Message, true, DateTime.Now, NetworkProtocolTypes.Message));
            await _chatt.SendMessageAsync(Message);
            Message = "";
            
        }


        private bool ShowMessageBox(string username)
        {
            bool isAcceptionConnection = MessageBox.Show($"Do you want to connect with {username}", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            return isAcceptionConnection;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void IChatt_MessageReviced(object sender, MessageEventArgs e)
        {
            _items.Add(new Message(e.Username,e.Message,false,DateTime.Now,NetworkProtocolTypes.Message));
        }

        private void IChattDisconnect(object sender)
        {
            
            canSeeChattHistory = true;
            OnPropertyChanged(nameof(CanSeeChatthistory));
            Func<ITcpClient> createClient = () => new TcpClientWrapper();
            Func<IPEndPoint, ITcpListener> createListner = (IPEndPoint endPoint) => new TcpListenerWrapper(endPoint.Address.ToString(), endPoint.Port);
            IDataSerialize<IMessage> dataSerialize = new DataSerializer<IMessage>();
            INetworkProtocol<IMessage> messageFactory = new MessageFactory(new DatetimeWrapper());
            IConservation<IMessage> Conservation = new Conservation<IMessage>(new MemoryStream(), dataSerialize);
            _chatt = new Chatt(new NetworkHandler<IMessage>(createListner, createClient, messageFactory, dataSerialize), Conservation);
            _chatt.SubscribeToMessage(IChatt_MessageReviced);
            _chatt.SubscribeToDisconnec(IChattDisconnect);
        }


    }
}
