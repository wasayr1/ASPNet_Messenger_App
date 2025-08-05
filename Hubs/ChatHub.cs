using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRMessenger.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string receiverId, string message)
        {
            // Get the ID of the user who is sending the message
            var senderId = Context.UserIdentifier;

            // "ReceiveMessage" is the name of the method that the client-side JS will be listening for.
            // We send the message content and the sender's ID so the client knows who it's from.

            // Send the message to the receiver
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);

            // Send the message back to the sender so their own screen updates
            await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, message);
        }
    }
}
