using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models.Enums;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models
{
    public class NetworkHandler<T> : IDisposable
    {

        private NetworkState state = NetworkState.Ideal;

        private INetworkProtocol<T> networkProtocol;

        private readonly Func<IPEndPoint,ITcpListener> createListerner; 

        private readonly Func<ITcpClient> createClient;

        private StreamReader? _reader;

        private StreamWriter? _writer;

        private ITcpClient? _client;

        private bool disposedValue;


        public NetworkHandler(Func<IPEndPoint,ITcpListener> createListerner, Func<ITcpClient> createClient,INetworkProtocol<T> networkProtocol )
        {
            this.createListerner = createListerner;
            this.createClient = createClient;
            this.networkProtocol = networkProtocol;
        }

        public NetworkHandler()
        {
            throw new InvalidOperationException("fdsfdsf");
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


        public void AccepetConnection()
        {
            if(state != NetworkState.WaitingForHandshake)
            {
                throw new InvalidOperationException("You can not accpetion a connetion if you are not wating");
            }
            state = NetworkState.Connected;
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

       
        public async IAsyncEnumerable<T?> GetMessages()
        {
            if(state != NetworkState.Connected)
            {

                throw new InvalidOperationException("The Networkhandler needs to be in the connetdate but its current in the ");
            }
            string? message = await ReadFromStreamAsync();
            T decodeMessage = await networkProtocol.DecodeStringeAsync(message);
            yield return decodeMessage; 
            await Task.Delay(5000);
        }


        public async Task SendMessage(string message)
        {
            if (state != NetworkState.Connected)
            {
                throw new InvalidOperationException("The Networkhandaler can only send message when its connected");
            }
            await WriteToStream(await networkProtocol.CreateMessageAsync(message));

        }

        public async Task Disconect()
        {
            if (state != NetworkState.Connected)
            {
                throw new InvalidOperationException("The NetworkHandler can only be discontect if its allready connetcd");
            }
            await SendDisconnectMessage();
            Dispose(true);
        }



        private async Task SendDisconnectMessage()
        {
            string disconnectMessage = await networkProtocol.CreateDisconnectStringAsync("Username");
            await WriteToStream(disconnectMessage);
        }

        private async Task<T?> EstablishConnection(string username)
        {

            state = NetworkState.WaitingForHandshake;
            await WriteToStream(await networkProtocol.CreateConnectionRequestStringAsync(username));
            string? response = await ReadFromStreamAsync();
            return await networkProtocol.DecodeStringeAsync(response);
        }



        private async Task WriteToStream(string message)
        {
            if (_writer is not null)
            {
                await _writer.WriteAsync(message);
                await _writer.FlushAsync();
            }
        }

        private async Task<string?> ReadFromStreamAsync()
        {
            if ( _reader is not null)
            {    
                return await _reader.ReadLineAsync();
            }
            throw new NullReferenceException("The ");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
             
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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
    }
}
