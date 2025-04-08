using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Models.Data;
using System.Security.Claims;

namespace SignalRChat.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("signin-google")]
        public IActionResult LoginByGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback", "Auth", null, Request.Scheme)
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("signin-google-callback")]
        public async Task<IActionResult> GoogleLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                return Unauthorized();
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var avatar = claims?.FirstOrDefault(c => c.Type == "picture")?.Value;
            var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (email == null || googleId == null)
            {
                return BadRequest("Invalid Google login response");
            }

            
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == googleId);
            if (existingUser == null)
            {
                var newUser = new User
                {
                    UserId = googleId,
                    FullName = name ?? "",
                    Email = email,
                    AvatarUrl = avatar ?? ""
                };

                _context.Users.Add(newUser);               
            }
            else
            {
                existingUser.FullName = name ?? existingUser.FullName;
                existingUser.Email = email ?? existingUser.Email;
                existingUser.AvatarUrl = avatar ?? existingUser.AvatarUrl;
            }

            await _context.SaveChangesAsync();

            return Redirect("/"); // Chuyển hướng về trang chủ sau khi đăng nhập thành công
        }


        [HttpGet("user")]
        public IActionResult GetUser()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var name = User.FindFirst(ClaimTypes.Name)?.Value;

                return Ok(new { email, name });
            }

            return Unauthorized();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
