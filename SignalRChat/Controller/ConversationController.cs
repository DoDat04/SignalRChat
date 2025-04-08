using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;
using SignalRChat.Models.Data;
using System.Security.Claims;

namespace SignalRChat.Controller
{
    [Route("/api/conversations")]
    public class ConversationController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ConversationController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpPost("start-conversation")]
        public async Task<IActionResult> StartConversation([FromBody] StartConversationRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(request.ReceiverId) || request.ReceiverId == currentUserId)
            {
                return BadRequest("Invalid request");
            }

            var existingConversation = await _context.Conversations
                .Include(c => c.Members)
                .Where(c => c.Members.Any(m => m.UserId == currentUserId) && c.Members.Any(m => m.UserId == request.ReceiverId))
                .FirstOrDefaultAsync();

            if (existingConversation != null)
            {
                return Ok(new { conversationId = existingConversation.ConversationId });
            }

            // Tạo mới cuộc hội thoại
            var newConversation = new Conversation
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Members = new List<ConversationMember>
        {
            new ConversationMember { UserId = currentUserId, JoinedAt = DateTime.Now },
            new ConversationMember { UserId = request.ReceiverId, JoinedAt = DateTime.Now }
        }
            };

            _context.Conversations.Add(newConversation);
            await _context.SaveChangesAsync();

            // Gửi cuộc hội thoại mới tới SignalR (lưu conversationId vào Context.Items)
            var chatHubContext = (IHubContext<ChatHub>)HttpContext.RequestServices.GetService(typeof(IHubContext<ChatHub>));
            await chatHubContext.Clients.User(currentUserId).SendAsync("ConversationStarted", newConversation.ConversationId);

            return Ok(new { conversationId = newConversation.ConversationId });
        }

        [HttpGet("messages/{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .Include(m => m.Sender) 
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            // Return messages with the User (Sender) FullName
            var result = messages.Select(m => new
            {
                m.MessageId,
                m.ConversationId,
                m.SenderId,
                m.MessageText,
                m.SentAt,
                m.MessageStatus,
                m.IsDeleted,
                SenderFullName = m.Sender.FullName 
            }).ToList();

            return Ok(result);
        }

        public class StartConversationRequest
        {
            public string ReceiverId { get; set; }
        }

    }
}
