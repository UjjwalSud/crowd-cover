using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using System.Net;

public class EmailSender : IEmailSender
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;

    public EmailSender(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _smtpUser = smtpUser;
        _smtpPass = smtpPass;

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CrowdCover Team", _smtpUser));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            // Connect to Gmail's SMTP server using SSL
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

            // Authenticate with the Gmail SMTP server
            await client.AuthenticateAsync(_smtpUser, _smtpPass);

            // Send the email
            await client.SendAsync(message);

            // Disconnect from the server
            await client.DisconnectAsync(true);
        }
    }
}
