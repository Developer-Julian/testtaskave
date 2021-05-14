using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskAve.WebClient.Hubs
{
    public class ChatHub: Hub
    {
        public async Task MessageReceived(string msg)
        {
            await Clients.All.SendAsync("MessageReceived", msg);
        }
    }
}
