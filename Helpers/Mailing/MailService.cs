using MimeKit;
using MailKit.Net.Smtp;

public class MailService
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly bool _enableSsl;

    public MailService(string smtpServer, int port, string username, string password, bool enableSsl)
    {
        _smtpServer = smtpServer;
        _port = port;
        _username = username;
        _password = password;
        _enableSsl = enableSsl;
    }

    public void SendEmail(string fromName, string fromEmail, string toName, string toEmail, string subject, string body, bool isBodyHtml = false)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = isBodyHtml ? body : null,
                TextBody = isBodyHtml ? null : body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_smtpServer, _port, _enableSsl);
                client.Authenticate(_username, _password);

                client.Send(message);
                client.Disconnect(true);
            }

            Console.WriteLine("Email sent successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send email: " + ex.ToString());
        }
    }
}