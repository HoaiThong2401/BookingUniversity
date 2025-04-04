namespace Services.IService;

public interface IEmailService
{
    Task<bool> SendEmailAsync(IEnumerable<string> toList, string subject, string body);
    Task SendEmailWithQrCodeAsync(string email, string subject, string body);
}