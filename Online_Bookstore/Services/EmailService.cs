using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class EmailService
    {
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService()
        {
            _fromEmail = ConfigurationManager.AppSettings["EmailFrom"];
            _password = ConfigurationManager.AppSettings["EmailPassword"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential(_fromEmail, _password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}