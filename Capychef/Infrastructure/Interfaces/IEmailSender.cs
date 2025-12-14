namespace Capychef.Infrastructure.Interfaces;

public interface IEmailSender
{
    Task SendEmailVerificationEmailAsync(string email, string token);
}