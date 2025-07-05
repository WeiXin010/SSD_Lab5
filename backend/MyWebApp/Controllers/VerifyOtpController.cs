using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerifyOtpController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<VerifyOtpController> _logger;

        public VerifyOtpController(AppDbContext db, ILogger<VerifyOtpController> logger)
        {
            _db = db;
            _logger = logger;

        }

        public class SubmittedOtp
        {
            public string email { get; set; } = null!;
            public string otp { get; set; } = null!;
            public DateTime submittedTime { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] SubmittedOtp submittedOtp)
        {
            _logger.LogInformation($"Time submitted {submittedOtp.submittedTime} for {submittedOtp.email}");
            // Find user by username
            var user = await _db.OtpRecords.SingleOrDefaultAsync(u => u.Email == submittedOtp.email);

            if (user == null)
            {
                return Unauthorized(new { message = "No login record. Please login again" });
            }

            if (!VerifyOtp(user.OtpCode, submittedOtp.otp, user.ExpireAt, submittedOtp.submittedTime))
            {
                return Unauthorized(new { message = "OPT is wrong or code expired!" });
            }


            return Ok(new { message = "Login Successful" });
        }

        // Currently is checking plaintext! NEED TO CHANGE
        private bool VerifyOtp(string otp, string submittedOtp, DateTime expiredAt, DateTime submittedAt)
        {
            return otp == submittedOtp & submittedAt <= expiredAt;
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
