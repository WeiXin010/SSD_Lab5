using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly string senderEmail = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
    private readonly string senderPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

    [HttpPost("send")]
    public IActionResult SendEmail([FromBody] EmailRequest request)
    {
        if (string.IsNullOrEmpty(request.ToEmail))
            return BadRequest("Recipient email is required.");

        try
        {
            var fromAddress = new MailAddress(senderEmail, "Your App Name");
            var toAddress = new MailAddress(request.ToEmail);
            var subject = "Test Email";
            var body = "This is a test email.";

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            smtp.Send(message);

            return Ok("Email sent successfully.");
        }
        catch (SmtpException ex)
        {
            return StatusCode(500, $"Email sending failed: {ex.Message}");
        }
    }
}