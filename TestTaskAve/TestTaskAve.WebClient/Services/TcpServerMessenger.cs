using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestTaskAve.WebClient.Hubs;

namespace TestTaskAve.WebClient.Services
{
    internal sealed class TcpServerMessenger: ITcpServerMessenger
    {
        private const string host = "127.0.0.1";
        private const int port = 8888;
        private readonly IMemoryCache cache;
        private readonly IHubContext<ChatHub> chatHub;

        public TcpServerMessenger(IMemoryCache cache, IHubContext<ChatHub> chatHub)
        {
            this.cache = cache;
            this.chatHub = chatHub;
        }

        public void Connect(string userName)
        {
            var client = new TcpClient();
            client.Connect(host, port);
            var stream = client.GetStream();
            var data = Encoding.Unicode.GetBytes(userName);
            stream.Write(data, 0, data.Length);
            this.cache.Set(userName, stream);

            var cancelTokenSource = new CancellationTokenSource();
            this.cache.Set($"{userName}-CancelationSource", cancelTokenSource);
            Task.Run(() => ListenAsync(stream, client, userName));
        }

        public void Disconnect(string userName)
        {
            var stream = (NetworkStream)this.cache.Get(userName);
            if (stream != null)
            {
                stream.Close();
            }

            var cancelTokenSource = (CancellationTokenSource)this.cache.Get($"{userName}-CancelationSource");
            if (cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
            }
        }

        public void SendMessage(string userName, string message)
        {
            var stream = (NetworkStream) this.cache.Get(userName);
            var data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private async Task ListenAsync(NetworkStream stream, TcpClient client, string userName)
        {
            while (true)
            {
                var cancelTokenSource = (CancellationTokenSource)this.cache.Get($"{userName}-CancelationSource");
                var token = cancelTokenSource.Token;
                if (token.IsCancellationRequested)
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }

                    if (client != null)
                    {
                        client.Close();
                    }

                    this.cache.Remove(userName);
                    this.cache.Remove($"{userName}-CancelationSource");

                    return;
                }

                try
                {
                    var dataBuffer = new byte[64];
                    var builder = new StringBuilder();
                    while (stream.DataAvailable && stream.CanRead)
                    {
                        var bytes = stream.Read(dataBuffer, 0, dataBuffer.Length);
                        if (bytes == 0)
                        {
                            break;
                        }

                        builder.Append(Encoding.Unicode.GetString(dataBuffer, 0, bytes));
                    }

                    var message = builder.ToString();
                    if (message.Length == 0)
                    {
                        continue;
                    }

                    if (message.StartsWith($"{userName}:"))
                    {
                        continue;
                    }

                    await this.chatHub.Clients.All.SendAsync("MessageReceived", message);
                }
                catch
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }

                    if (client != null)
                    {
                        client.Close();
                    }

                    this.cache.Remove(userName);
                }
            }
        }
    }
}
