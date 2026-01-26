using System.ComponentModel.DataAnnotations;

namespace Capychef.Users.Domain.Cmd;

public class SignupCmd
{
    [MinLength(1)] public string Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}