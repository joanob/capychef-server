using Capychef.Common.Errors;
using Capychef.Common.Interfaces;
using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.Cmd;

public class ChangePasswordCmd : ICmd
{
    public required string CurrentPassword { get; set; }

    public required string NewPassword { get; set; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrEmpty(CurrentPassword))
            return new ValidationError("ChangePasswordCmd CurrentPassword is empty");

        if (string.IsNullOrEmpty(NewPassword))
            return new ValidationError("ChangePasswordCmd NewPassword is empty");

        if (CurrentPassword == NewPassword)
            return new ValidationError("ChangePasswordCmd NewPassword must be different from CurrentPassword");

        return UserPassword.ValidatePassword(NewPassword);
    }
}