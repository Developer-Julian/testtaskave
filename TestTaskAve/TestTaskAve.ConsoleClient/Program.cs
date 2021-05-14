using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestTaskAve.ConsoleClient
{
    class Program
    {
        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

        static void Main(string[] args)
        {
            Console.Write("Введите свое имя: ");
            userName = Console.ReadLine();
            client = new TcpClient();
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();

                string message = userName;
                var data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                var receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                Console.WriteLine("Добро пожаловать, {0}", userName);
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        static void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            while (true)
            {
                string message = Console.ReadLine();
                var data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }

        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    var data = new byte[64];
                    var builder = new StringBuilder();
                    while (stream.DataAvailable && stream.CanRead)
                    {
                        var bytes = stream.Read(data, 0, data.Length);
                        if (bytes == 0)
                        {
                            break;
                        }

                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }

                    var message = builder.ToString();
                    if (message.Length == 0)
                    {
                        continue;
                    }

                    Console.WriteLine(message);
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            Environment.Exit(0);
        }
    }
}
