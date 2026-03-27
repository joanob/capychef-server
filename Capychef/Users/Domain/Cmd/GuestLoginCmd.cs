using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Users.Domain.Cmd;

public class GuestLoginCmd : ICmd
{
    public required string GuestTransferenceToken { get; init; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrEmpty(GuestTransferenceToken))
            return new ValidationError("GuestLoginCmd GuestTransferenceToken is empty");

        return null;
    }
}