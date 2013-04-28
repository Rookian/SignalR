using System.Threading;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            for (int i = 0; i < 100; i++)
            {
                Clients.All.broadcastMessage(name, "#" + i + " " + message);
                Thread.Sleep(1000);
            }           
        }        
    }
}