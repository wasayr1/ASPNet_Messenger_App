using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRMessenger.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string receiverId, string message)
        {
            await Clients.User(receiverId).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
        }
    }
}
