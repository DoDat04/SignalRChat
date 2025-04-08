using Microsoft.VisualBasic;

namespace SignalRChat.Models.Data
{
    public class ConversationMember
    {
        public int ConversationId { get; set; }
        public string UserId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public Conversation Conversation { get; set; }
        public User User { get; set; }
    }
}
