using System.ComponentModel.DataAnnotations;

namespace SignalRChat.Models.Data
{
    public class User
    {
        [Key]
        public string UserId { get; set; } // Google ID
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }

        public ICollection<ConversationMember> ConversationMembers { get; set; }
    }
}
