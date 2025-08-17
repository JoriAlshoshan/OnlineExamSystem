using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace onlineExamApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        // NOTE: This email configuration uses my personal {Mailtrap} account
        // for testing email functionality only.
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_config["EmailSettings:Host"])
            {
                Port = int.Parse(_config["EmailSettings:Port"]),
                Credentials = new NetworkCredential(
                    _config["EmailSettings:Username"],
                    _config["EmailSettings:Password"]
                ),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["EmailSettings:From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            // Send the password reset link to the user's email
            // using my personal Mailtrap account for testing purposes only

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
