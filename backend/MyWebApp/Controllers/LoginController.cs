using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<LoginController> _logger;

        public LoginController(AppDbContext db, ILogger<LoginController> logger)
        {
            _db = db;
            _logger = logger;

        }

        public class LoginRequest
        {
            public string email { get; set; } = null!;
            public string password { get; set; } = null!;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"Login attempt for {request.email}");
            // Find user by username
            var user = await _db.Users.SingleOrDefaultAsync(u => u.email == request.email);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid Username or Password" });
            }

            if (!VerifyPassword(user.password, request.password))
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
