using SignalRMessenger.Models;

namespace SignalRMessenger.ViewModels
{
    public class ChatViewModel
    {
        public ApplicationUser Receiver { get; set; }
        public List<Message> Messages { get; set; }
    }
}
