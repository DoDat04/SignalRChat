using Microsoft.EntityFrameworkCore;
using SignalRChat.Models.Data;

namespace SignalRChat.Services
{
    public class MessageService
    {
        private readonly AppDbContext _context;
        public MessageService(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task SaveMessageAsync(string senderId, string messageText, int conversationId)
        {
            var message = new Message
            {
                SenderId = senderId,
                MessageText = messageText,
                ConversationId = conversationId,
                SentAt = DateTime.Now,
                MessageStatus = "UNREAD",
                IsDeleted = false
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
