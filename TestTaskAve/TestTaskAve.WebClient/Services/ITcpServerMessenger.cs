using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskAve.WebClient.Services
{
    public interface ITcpServerMessenger
    {
        void Connect(string userName);
        void Disconnect(string userName);
        void SendMessage(string userName, string message);
    }
}
