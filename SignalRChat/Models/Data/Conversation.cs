using System.ComponentModel.DataAnnotations;

namespace SignalRChat.Models.Data
{
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<ConversationMember> Members { get; set; }
    }
}
