using Capychef.Common.Errors;
using Capychef.Common.Interfaces;
using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.Cmd;

public class ResetPasswordCmd : ICmd
{
    public string? PasswordRecoveryToken { get; set; }

    public string Password { get; set; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrEmpty(PasswordRecoveryToken))
            return new ValidationError("ResetPasswordCmd PasswordRecoveryToken is empty");

        if (string.IsNullOrEmpty(Password))
            return new ValidationError("ResetPasswordCmd Password is required");

        var error = UserPassword.ValidatePassword(Password);
        if (error != null)
            return error;

        return null;
    }
}