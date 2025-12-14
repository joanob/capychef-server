using Capychef.Infrastructure.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace Capychef.Infrastructure.DevImplementations;

public class Smtp4DevSender : IEmailSender
{
    public async Task SendEmailVerificationEmailAsync(string email, string token)
    {
        var msg = new MimeMessage();

        msg.From.Add(new MailboxAddress("Capychef", "no-reply@capychef.com"));
        msg.To.Add(new MailboxAddress("", email));

        msg.Subject = "Verifica tu email - Capychef";
        msg.Body = new TextPart("plain") { Text = token };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("localhost", 2525, false);
        await smtp.SendAsync(msg);
        await smtp.DisconnectAsync(true);
    }
}