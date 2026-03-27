using Capychef.Common.Errors;
using Capychef.Common.Interfaces;
using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.Cmd;

public class ResetPasswordCmd : ICmd
{
    public required string PasswordRecoveryToken { get; set; }

    public required string Password { get; set; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrEmpty(PasswordRecoveryToken))
            return new ValidationError("ResetPasswordCmd PasswordRecoveryToken is empty");

        if (string.IsNullOrEmpty(Password))
            return new ValidationError("ResetPasswordCmd Password is required");

        var error = UserPassword.ValidatePassword(Password);

        return error;
    }
}