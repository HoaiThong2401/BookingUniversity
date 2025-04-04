using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.IService;

namespace Services.Service;

public class EmailService : IEmailService
{
     private readonly SmtpClient _smtpClient;
        private readonly string _senderEmail;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _senderEmail = configuration["EmailSettings:Sender"] ?? "";
            string? password = configuration["EmailSettings:Password"];
            string? host = configuration["EmailSettings:Host"];
            int port = int.Parse(configuration["EmailSettings:Port"] ?? "587");

            _smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderEmail, password)
            };

            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(IEnumerable<string> toList, string subject, string body)
        {
            try
            {
                foreach (var to in toList)
                {
                    var mailMessage = new MailMessage(_senderEmail, to, subject, body);
                    await _smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email.");
                return false;
            }

            return true;
        }
            public async Task SendEmailWithQrCodeAsync(string email, string subject, string body)
        {
            if(!string.IsNullOrWhiteSpace(email))
            {
                // Create an email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = subject,
                    Body = $@"
                        <html>
                        <body>
                            <p>{body}</p>
                            <p>Scan the QR code below:</p>
                            <img src='cid:QrCodeImage' alt='QR Code' />
                        </body>
                        </html>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                await _smtpClient.SendMailAsync(mailMessage);
            }
        }
}