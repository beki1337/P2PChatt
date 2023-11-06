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
    public class NetworkHandler : IDisposable
    {

        private NetworkState state = NetworkState.Ideal;

        private readonly Func<IPEndPoint,ITcpListener> createListerner; 

        private readonly Func<ITcpClient> createClient;

        private StreamReader? _reader;

        private StreamWriter? _writer;

        private ITcpClient _client;

        private bool disposedValue;


        public NetworkHandler(Func<IPEndPoint,ITcpListener> createListerner, Func<ITcpClient> createClient)
        {
            this.createListerner = createListerner;
            this.createClient = createClient;   
        }

        public async Task<string> Connect(string ipAddress, int port)
        {
            // Change the state 
            state = NetworkState.Connected;

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
                string? username = await ReadFromStreamAsync();
                if (string.IsNullOrEmpty(username))
                {
                    throw new InvalidOperationException("The received username was null or empty.");
                }

                return username;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Connection error", ex);
            }
        }

        public async Task<string> Listen(string IpAddres, int port)
        {
            throw new NotImplementedException();
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
