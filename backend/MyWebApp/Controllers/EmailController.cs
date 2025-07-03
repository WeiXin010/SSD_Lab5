using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Identity.Data;


[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly ILogger<EmailController> _logger;

    public EmailController(ILogger<EmailController> logger)
    {
        _logger = logger;
    }


    [HttpPost]
    public IActionResult SendEmail([FromBody] EmailRequest request)
    {
        if (string.IsNullOrEmpty(request.ToEmail))
        {
            return BadRequest("Recipient email is required");
        }

        try
        {
            var senderEmail = Environment.GetEnvironmentVariable("EMAIL_ADDRESS") ?? throw new InvalidOperationException("EMAIL_ADDRESS environment variable is not set");
            var senderPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? throw new InvalidOperationException("EMAIL_PASSWORD environment variable is not set"); ;

            var message = new MailMessage();
            message.From = new MailAddress(senderEmail, "Ready4Work");
            message.To.Add(new MailAddress(request.ToEmail));
            message.Subject = "You've been accepted!";
            message.Body = "Hi there! Congratulations, you've been accepted to Company A.";
            message.IsBodyHtml = false;

            // Add headers
            message.ReplyToList.Add(new MailAddress(senderEmail));
            message.Headers.Add("X-Priority", "1");
            message.Headers.Add("X-MSMail-Priority", "High");

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
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