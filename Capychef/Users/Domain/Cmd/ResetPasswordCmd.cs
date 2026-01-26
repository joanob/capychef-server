namespace Capychef.Users.Domain.Cmd;

public class ResetPasswordCmd
{
    public string? PasswordRecoveryToken { get; set; }

    public string Password { get; set; }
}