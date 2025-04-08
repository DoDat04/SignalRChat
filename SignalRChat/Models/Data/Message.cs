using System.ComponentModel.DataAnnotations;

namespace SignalRChat.Models.Data
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public string SenderId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string MessageStatus { get; set; } = "UNREAD"; // READ / UNREAD / DELETED
        public bool IsDeleted { get; set; } = false; // Nếu muốn ẩn tin nhắn

        public Conversation Conversation { get; set; }
        public User Sender { get; set; }
    }
}
