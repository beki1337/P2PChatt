using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TDDD49Lab.Models.Enums;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    public class NetworkHandler<T> : INetworkHandler<T> where T : IMessage

    {

        private NetworkState state = NetworkState.Ideal;

        private readonly INetworkProtocol<T> networkProtocol;

        private readonly IDataSerialize<IMessage> dataSerialize;

        private readonly Func<IPEndPoint,ITcpListener> createListerner;

        private CancellationTokenSource cancellationTokenSource;

        private readonly Func<ITcpClient> createClient;

        private StreamReader? _reader;

        private StreamWriter? _writer;

        private ITcpClient? _client;

        private bool disposedValue = false;

      


        public NetworkHandler(Func<IPEndPoint,ITcpListener> createListerner, Func<ITcpClient> createClient,INetworkProtocol<T> networkProtocol, IDataSerialize<IMessage> dataSerialize )
        {
            this.createListerner = createListerner;
            this.createClient = createClient;
            this.networkProtocol = networkProtocol;
            this.dataSerialize = dataSerialize;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public NetworkState CurrentNetworkState()
        {
            return state;
        }

       

        public async Task<T> Connect(string ipAddress, int port)
        {
            // Change the state 
            if(state != NetworkState.Ideal)
            {
                throw new InvalidOperationException("The networkhandler can only connect if the handler is in ideal model");
            }
            state = NetworkState.Listening;

            // Validate the IP address
            if (!IPAddress.TryParse(ipAddress, out IPAddress? parsedAddress))
            {
                throw new ArgumentException("Invalid IP address format.");
            }

            var ipEndPoint = new IPEndPoint(parsedAddress, port);

             _client = createClient();

            try
            {
                // Connect asynchronously
                await _client.ConnectAsync(ipEndPoint);

                // Initialize the reader and writer
                _reader = new StreamReader(_client.GetStream());
                _writer = new StreamWriter(_client.GetStream());

                // Read and return the username
               

                T? username = await EstablishConnection("Seraching");
             
                if (username is null)
                {
                    throw new InvalidOperationException($"The received username was null or empty. {username}");
                }

                return username;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Connection error", ex);
            }
        }


        public async Task<bool> AcceptConnection()
        {
            if(state != NetworkState.WaitingForHandshake)
            {
                throw new InvalidOperationException("You can not accpetion a connetion if you are not wating");
            }

            var accepetMessage = networkProtocol.CreateAcceptHandshake("Username");
            await WriteToStream(accepetMessage);
            MessageBox.Show("Wea are in the chatt appecet sate");
            string? response = await _reader.ReadLineAsync();

            if (response is null)
            {
                throw new NullReferenceException("The reviceb message was null");

            }

           IMessage message =  dataSerialize.DeserializeFromFormat<Message>(response);

            if(message.NetworkProtocolType == NetworkProtocolTypes.DeniedConnection)
            {

                state = NetworkState.Ideal;
                return false;
            } else if (message.NetworkProtocolType == NetworkProtocolTypes.AcceptConnection)
            {
                state = NetworkState.Connected;
                return true;
            }
            throw new InvalidOperationException("Inavild Networkprotcol type recived");
        }

        public async Task DeclineRequest()
        {
            if (state != NetworkState.WaitingForHandshake)
            {
                throw new InvalidOperationException("You can not accpetion a connetion if you are not wating");
            }
            var declinemessge = networkProtocol.CreateDeclineHandshake("Username");
            await WriteToStream(declinemessge);
            state = NetworkState.Ideal;
        }

        public async Task<T> Listen(string IpAddres, int port)
        {
            if(state != NetworkState.Ideal)
            {
                throw new InvalidOperationException("You can only start listing when the network handler is in the Ideal state");
            }
            state = NetworkState.Listening;
            // Validate the IP address
            if (!IPAddress.TryParse(IpAddres, out IPAddress? parsedAddress))
            {
                throw new ArgumentException("Invalid IP address format.");
            }

            var ipEndPoint = new IPEndPoint(parsedAddress, port);

            ITcpListener listener = createListerner(ipEndPoint);

            listener.Start();
          
            _client = await listener.AcceptTcpClientAsync();
            listener.Stop();
            _reader = new StreamReader(_client.GetStream());
            _writer = new StreamWriter(_client.GetStream());
          
            T? username = await EstablishConnection("Listern");
            
            if (username is null)
            {
                throw new InvalidOperationException($"The received username was null or empty. {username}");
            }

            return username;
        }

       
        public async IAsyncEnumerable<T?> GetMessages([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            
            if (state != NetworkState.Connected)
            {

                throw new InvalidOperationException($"The Networkhandler needs to be in the connetdate but its current in the {state}");
            }


            while (!cancellationToken.IsCancellationRequested)
            {
                string? message = await ReadFromStreamAsync(); // Assuming this method reads a message from the stream

                if (message == null)
                {
                    await Task.Delay(1000, cancellationToken); // Add a delay before attempting to read again
                    break; // Skip the rest of the loop and try to read another message
                }

                T decodeMessage = (T)dataSerialize.DeserializeFromFormat<Message>(message);
                yield return decodeMessage;
            }
    
        }


        public async Task SendMessage(string message)
        {
            if (state != NetworkState.Connected)
            {
                throw new InvalidOperationException("The Networkhandaler can only send message when its connected");
            }
          
            T messageToSend = networkProtocol.CreateMessage("sending user", message);
            await WriteToStream(messageToSend);
            MessageBox.Show("Vi skicker du ett nytt meddelande");

        }

        public async Task Disconect()
        {
            if (state != NetworkState.Connected)
            {
                throw new InvalidOperationException("The NetworkHandler can only be discontect if its allready connetcd");
            }
            
            await SendDisconnectMessage();
            state = NetworkState.Ideal;
            Dispose(true);
        }



        private async Task SendDisconnectMessage()
        {
            await WriteToStream(networkProtocol.CreateDisconnect("Username"));
        }

        private async Task<T?> EstablishConnection(string username)
        {
            
            state = NetworkState.WaitingForHandshake;
            await WriteToStream(networkProtocol.CreateConnectionHandshake(username));
            
            string? response = await _reader.ReadLineAsync();
            if (response == null)
            {
                throw new InvalidOperationException("The retrived username was null");
            }
            T message = (T)dataSerialize.DeserializeFromFormat<Message>(response);
            return message;
          
        }



        private async Task WriteToStream(T message)
        {
            if (_writer is not null)
            {
               
                string messageToSend = dataSerialize.SerializeToFormat(message);
                await _writer.WriteLineAsync(messageToSend);
                await _writer.FlushAsync();
              
            } else
            {
                throw new NullReferenceException("The currenet stream write is null");
            }
        }

        private async Task<string?> ReadFromStreamAsync()
        {
           
            if ( _reader is not null && !disposedValue)
            {
                try
                {
                    return await _reader.ReadLineAsync(cancellationTokenSource.Token);

                } catch (OperationCanceledException ) 
                {
                    Dispose(disposedValue);
                }
            }return null;
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                cancellationTokenSource.Cancel();

                _writer?.Close();
                _reader?.Close();
               

                _writer?.Dispose();
                _reader?.Dispose();
                _client?.Dispose();
          
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
         ~NetworkHandler()
         {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(disposing: false);
         }

        public void Dispose()
        {
            
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        NetworkState INetworkHandler<T>.CurrentNetworkStatet()
        {
            return state;
        }
    }
}
