using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Identity.Data;


[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly string senderEmail = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
    private readonly string senderPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

    [HttpPost]
    public IActionResult SendEmail([FromBody] EmailRequest request)
    {
        if (string.IsNullOrEmpty(request.ToEmail))
        {
            return BadRequest("Recipient email is required");
        }

        try
        {
            var fromAddress = new MailAddress(senderEmail, "Ready4work");
            var toAddress = new MailAddress(request.ToEmail);
            var subject = "Job notification";
            var body = "This is to inform you that you have been accepted by Company A";


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
        catch (SmtpException ex){
            return StatusCode(500, $"Email sending failed: {ex.Message}");
        }
    }
}