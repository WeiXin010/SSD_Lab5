using SendGrid;
using SendGrid.Helpers.Mail;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendOtpEmailAsync(string toEmail, string otpCode)
    {
        var emailApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        if (string.IsNullOrWhiteSpace(emailApiKey)) throw new InvalidOperationException("Missing SendGrid API key.");

        var client = new SendGrid.SendGridClient(emailApiKey);
        var from = new EmailAddress("noreply.ready4work@gmail.com", "Ready4Work");
        var to = new EmailAddress(toEmail);
        var subject = "Verify your login";
        var plainTextContent = $"Your OTP code is: {otpCode}";
        var htmlContent = $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Make sure that you are the one that request for this OTP.</h2>
                    <p>OTP code: <strong>{otpCode}</strong></p>
                </body>
            </html>";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Body.ReadAsStringAsync();
            _logger.LogError("SendGrid error: {0}", errorBody);
            throw new Exception("SendGrid failed to send email.");
        }
    }
}