using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestTaskAve.Server
{
    class Program
    {
        static Thread listenThread;
        static void Main(string[] args)
        {
            var serverSession = new ServerSession();
            try
            {
                listenThread = new Thread(new ThreadStart(serverSession.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                serverSession.Disconnect();
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                var message = Console.ReadLine();
                if (message.Equals("exit"))
                {
                    serverSession.Disconnect();
                    Environment.Exit(0);
                }

                if (message.Equals("Is"))
                {
                    var sessions = serverSession.GetClientSessions();
                    sessions.ForEach(x =>
                    {
                        Console.WriteLine(x.Name);
                    });
                }
            }
        }
    }
}
