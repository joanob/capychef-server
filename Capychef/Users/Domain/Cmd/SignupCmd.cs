using System.ComponentModel.DataAnnotations;

namespace Capychef.Users.Domain.Cmd.Auth;

public class SignupCmd
{
    [MinLength(1)] public string Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}