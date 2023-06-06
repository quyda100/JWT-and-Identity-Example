using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using auth.Interfaces;

public class EmailService : IEmailService
{
    public void SendEmail(string from, string to, string subject, string body)
    {
        
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", from));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("your-email@example.com", "your-email-password");

                client.Send(message);
                client.Disconnect(true);
            }
        
    }
}