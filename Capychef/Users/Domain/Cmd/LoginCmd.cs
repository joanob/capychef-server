using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Users.Domain.Cmd;

public class LoginCmd : ICmd
{
    public required string Username { get; set; }

    public required string Password { get; set; }

    public ValidationError? Validate()
    {
        Username = Username.Trim();

        Password = Password.Trim();

        if (string.IsNullOrEmpty(Username)) return new ValidationError("LoginCmd username is empty");

        if (string.IsNullOrEmpty(Password)) return new ValidationError("LoginCmd password is empty");

        return null;
    }
}