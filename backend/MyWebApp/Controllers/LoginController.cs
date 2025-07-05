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
        private readonly IEmailService _emailService;

        public LoginController(AppDbContext db, ILogger<LoginController> logger, IEmailService emailService)
        {
            _db = db;
            _logger = logger;
            _emailService = emailService;
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

            string otpCode = GenerateOTP();

            // Save OTp to DB
            var otp = new OtpRecord
            {
                Email = request.email,
                OtpCode = otpCode,
                ExpireAt = DateTime.UtcNow.AddMinutes(5)
            };

            _db.OtpRecords.Add(otp);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"OTP for {request.email}: {otpCode}");
            await _emailService.SendOtpEmailAsync(request.email, otpCode);

            return Ok(new { message = "Login Successful" });
        }

        // Currently is checking plaintext! NEED TO CHANGE
        private bool VerifyPassword(string plainPassword, string storePassword)
        {
            return plainPassword == storePassword;
        }

        private string GenerateOTP()
        {
            var rng = new Random();
            string otp = "";
            for (int i = 0; i < 6; i++)
                otp += rng.Next(0, 10).ToString();
            return otp;
        }
    }
}
