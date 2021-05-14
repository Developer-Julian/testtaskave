using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TestTaskAve.Server
{
    internal sealed class ClientSession
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public NetworkStream Stream { get; private set; }

        private readonly TcpClient tcpClient;

        private readonly ServerSession serverSession;

        private readonly List<string> existingMessages;

        public ClientSession(TcpClient tcpClient, ServerSession serverSession, List<string> existingMessages)
        {
            this.Id = Guid.NewGuid().ToString();
            this.tcpClient = tcpClient;
            this.serverSession = serverSession;
            this.serverSession.AddConnection(this);
            this.existingMessages = existingMessages;
        }

        public void Process()
        {
            try
            {
                this.Stream = this.tcpClient.GetStream();

                var message = GetMessage();
                this.Name = message;

                message = this.Name + " вошел в чат";

                this.serverSession.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);

                
                this.existingMessages.ForEach(x =>
                {
                    var messageInBytes = Encoding.Unicode.GetBytes(x);
                    this.Stream.Write(messageInBytes, 0, messageInBytes.Length);
                });

                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        if (message.Length == 0)
                        {
                            continue;
                        }

                        message = $"{this.Name}: {message}";
                        Console.WriteLine(message);
                        serverSession.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = string.Format("{0}: покинул чат", this.Name);
                        Console.WriteLine(message);
                        serverSession.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                serverSession.RemoveConnection(this.Id);
                this.Close();
            }
        }

        private string GetMessage()
        {
            var data = new byte[64];
            var builder = new StringBuilder();
            while (Stream.DataAvailable && Stream.CanRead)
            {
                var bytes = Stream.Read(data, 0, data.Length);
                if (bytes == 0)
                {
                    break;
                }

                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }

            return builder.ToString();
        }

        public void Close()
        {
            if (this.Stream != null)
            {
                this.Stream.Close();
            }

            if (this.tcpClient != null)
            {
                this.tcpClient.Close();
            }
        }
    }
}
