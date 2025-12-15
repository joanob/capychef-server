namespace Capychef.Users.Domain.Cmd.Auth;

public class ResetPasswordCmd
{
    public string? PasswordRecoveryToken { get; set; }

    public string Password { get; set; }
}