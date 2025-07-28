using System.ComponentModel.DataAnnotations;

namespace SignalRMessenger.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
    }
}
