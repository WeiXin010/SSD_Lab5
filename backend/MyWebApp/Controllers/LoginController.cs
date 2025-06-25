using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _db;

        public LoginController(AppDbContext db)
        {
            _db = db;
        }

        public class LoginRequest
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Find user by username
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid Username or Password" });
            }

            if (!VerifyPassword(user.Password, request.Password))
            {
                return Unauthorized(new { message = "Invalid Username or Password" });
            }

            return Ok(new { message = "Login Successful" });
        }

        // Currently is checking plaintext! NEED TO CHANGE
        private bool VerifyPassword(string plainPassword, string storePassword)
        {
            return plainPassword == storePassword;
        }
    }
}
