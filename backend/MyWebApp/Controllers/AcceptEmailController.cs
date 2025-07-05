using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Identity.Data;
using SendGrid;
using SendGrid.Helpers.Mail;



[ApiController]
[Route("api/[controller]")]
public class AcceptEmailEmailController : ControllerBase
{
    private readonly ILogger<AcceptEmailEmailController> _logger;

    public AcceptEmailEmailController(ILogger<AcceptEmailEmailController> logger)
    {
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ToEmail))
            return BadRequest("Recipient email is required.");

        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
            return StatusCode(500, "Missing SendGrid API Key.");

        try
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply.ready4work@gmail.com", "DiCE");
            var to = new EmailAddress(request.ToEmail);
            var subject = "You've been accepted!";
            var plainTextContent = "You've been accepted by Company A";
            var htmlContent = @"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>🎉 Congratulations</h2>
                    <p>Hi there</p>
                    <p>We’re excited to let you know that you’ve been <strong>accepted to Company A</strong>!</p>
                    <p style='margin-top: 20px;'>– The Ready4Work Team</p>
                </body>
            </html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
                return Ok("Email sent successfully via SendGrid.");

            var errorBody = await response.Body.ReadAsStringAsync();
            _logger.LogError("SendGrid failed with status {Status}: {Body}", response.StatusCode, errorBody);
            return StatusCode((int)response.StatusCode, "SendGrid failed to send email.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending email.");
            return StatusCode(500, "An error occurred while sending email.");
        }
    }
}