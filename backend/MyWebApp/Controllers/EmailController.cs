using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Identity.Data;
using SendGrid;
using SendGrid.Helpers.Mail;



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
            var subject = "WARNING on Failure to submit!";
            var plainTextContent = "Chigga Chigga Chigga";
            var htmlContent = @"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>🎉 Gangstar Rap!</h2>
                    <p>Chigga Chigga Chigga</p>
                    <p>Imma <strong>100%</strong>chigga!</p>
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