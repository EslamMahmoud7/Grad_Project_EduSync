using Grad_Project_LMS.Interfaces;
using System.Net.Mail;
using System.Net;

namespace Grad_Project_LMS.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(string ToEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["EmailConfiguration:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailConfiguration:Port"] ?? "hello"),
                Credentials = new NetworkCredential(
                _configuration["EmailConfiguration:Username"],
                _configuration["EmailConfiguration:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailConfiguration:SenderEmail"] ?? "hello", _configuration["EmailConfiguration:SenderName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(ToEmail);
            await smtpClient.SendMailAsync(mailMessage);

        }
    }
}
