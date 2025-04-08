using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Models.Data;
using SignalRChat.Services;
using System.Security.Claims;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(string message, int conversationId)
        {
            var currentUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var fullName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            if (currentUserId == null) return;

            await _messageService.SaveMessageAsync(currentUserId, message, conversationId);

            await Clients.All.SendAsync("ReceiveMessage", fullName, message);
        }
    }
}
