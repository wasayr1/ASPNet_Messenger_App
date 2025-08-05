using SignalRMessenger.Models;

namespace SignalRMessenger.ViewModels
{
    public class ChatViewModel
    {
        public ApplicationUser Receiver { get; set; } = null!;
        public List<Message> Messages { get; set; } = null!;
    }
}
