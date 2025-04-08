using System.ComponentModel.DataAnnotations;

namespace SignalRChat.Models.Data
{
    public class OnlineUser
    {
        [Key]
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime LastActive { get; set; } = DateTime.Now;

        public User User { get; set; }
    }
}
