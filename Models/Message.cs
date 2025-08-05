using System.ComponentModel.DataAnnotations;

namespace SignalRMessenger.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }

        public string SenderId { get; set; } = null!;
        public virtual ApplicationUser Sender { get; set; } = null!;

        public string ReceiverId { get; set; } = null!; 
        public virtual ApplicationUser Receiver { get; set; } = null!;
    }
}
