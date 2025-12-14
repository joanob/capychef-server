using Capychef.Infrastructure.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace Capychef.Infrastructure.DevImplementations;

public class Smtp4DevSender : IEmailSender
{
    public async Task SendEmailVerificationEmailAsync(string email, string token)
    {
        var dest = new MailboxAddress("", email);
        var subject = "Verifica tu email - Capychef";
        var body = token;

        await SendEmail(dest, subject, body);
    }

    public async Task SendPasswordRecoveryEmailAsync(string email, string token)
    {
        var dest = new MailboxAddress("", email);
        var subject = "Reinicia tu contraseña - Capychef";
        var body = token;

        await SendEmail(dest, subject, body);
    }

    private async Task SendEmail(MailboxAddress dest, string subject, string body)
    {
        var msg = new MimeMessage();

        msg.From.Add(new MailboxAddress("Capychef", "no-reply@capychef.com"));
        msg.To.Add(dest);

        msg.Subject = subject;
        msg.Body = new TextPart("plain") { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("localhost", 2525, false);
        await smtp.SendAsync(msg);
        await smtp.DisconnectAsync(true);
    }
}