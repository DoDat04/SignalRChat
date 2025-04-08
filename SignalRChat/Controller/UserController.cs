using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SignalRChat.Controller
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }

            var users = await _context.Users
                .Where(u => u.FullName.Contains(query) || u.Email.Contains(query))
                .Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Email,
                    u.AvatarUrl
                }).ToListAsync();

            return Ok(users);
        }

        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var fullName = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userId == null)
            {
                return Unauthorized(new { message = "User chưa đăng nhập" });
            }

            return Ok(new
            {
                UserId = userId,
                FullName = fullName,
                Email = email
            });
        }
    }
}
