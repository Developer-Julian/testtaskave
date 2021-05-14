using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestTaskAve.Server
{
    internal sealed class ServerSession
    {
        private static TcpListener tcpListener;
        private List<ClientSession> clients = new List<ClientSession>();
        private LimitedQueue<string> messages = new LimitedQueue<string>(30);

        public void AddConnection(ClientSession clientSession)
        {
            clients.Add(clientSession);
        }

        public List<ClientSession> GetClientSessions()
        {
            return this.clients;
        }

        public void RemoveConnection(string id)
        {
            var client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
            }
        }

        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    var tcpClient = tcpListener.AcceptTcpClient();

                    var clientSession = new ClientSession(tcpClient, this, this.messages.GetElements());
                    var clientThread = new Thread(new ThreadStart(clientSession.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        public void BroadcastMessage(string message, string id)
        {
            this.SaveMessage(message);
            var data = Encoding.Unicode.GetBytes(message);
            for (var i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id)
                {
                    clients[i].Stream.Write(data, 0, data.Length);
                }
            }
        }

        public void Disconnect()
        {
            tcpListener.Stop();

            for (var i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }

            Environment.Exit(0);
        }

        private void SaveMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            this.messages.Enqueu($"{message}{Environment.NewLine}");
        }
    }
}
