using MailKit.Net.Smtp;
using MailKit;
using MailKit.Security;
using MimeKit;

namespace LightController.Classes
{
    public class EmailSender : IEmailSender
    {
        private readonly string _email, _password, _smtp;
        private readonly int _port;
        private readonly SecureSocketOptions _secureSocketOptions;
        
        public EmailSender(string email, string password, string smtp, int port = 25, SecureSocketOptions secureSocketOptions = SecureSocketOptions.None)
        {
            _email = email;
            _password = password;
            _smtp = smtp;
            _port = port;
            _secureSocketOptions = secureSocketOptions;
        }

        public void SendMessage(string recipient, string title, string body)
        {
            
            var message = new MimeMessage();
            var from = new MailboxAddress(_email, _email);
            var to = new MailboxAddress(recipient, recipient);


            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = title;
            message.Body = bodyBuilder.ToMessageBody();


            var client = new SmtpClient();
            client.Connect(_smtp, _port, _secureSocketOptions);
            client.Authenticate(_email, _password);
            
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
    
        }
        
        
    }
}