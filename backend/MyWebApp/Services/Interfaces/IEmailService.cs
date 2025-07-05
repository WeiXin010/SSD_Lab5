public interface IEmailService
{
    Task SendOtpEmailAsync(string toEmailm, string otpCode);
}