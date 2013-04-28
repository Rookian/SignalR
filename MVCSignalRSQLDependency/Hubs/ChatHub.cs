using Microsoft.AspNet.SignalR;

namespace MVCSignalRSQLDependency.Hubs
{
    public class ChatHub : Hub
    {
        volatile private static Repository _repository;

        public ChatHub()
        {
            _repository = new Repository();
        }

        public static void Show()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.displayStatus(_repository.Get());
        }
    }
}