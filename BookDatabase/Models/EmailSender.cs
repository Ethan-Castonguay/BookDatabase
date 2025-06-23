using System.Net.Mail;
using System.Net;

namespace BookDatabase.Models
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var from = "test@example.com";
            var username = "8ca125fc178709"; 
            var password = "d5904f493e42dc"; 

            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password)
            };

            var message = new MailMessage(from, email, subject, body);

            try
            {
                await client.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("SMTP ERROR:");
                Console.WriteLine("Status Code: " + ex.StatusCode);
                Console.WriteLine("Message: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner: " + ex.InnerException.Message);
                throw;
            }
        }

    }
}
