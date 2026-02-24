using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Users.Domain.Cmd;

public class GuestLoginCmd : ICmd
{
    public string GuestTransferenceToken { get; set; }


    public ValidationError? Validate()
    {
        if (string.IsNullOrEmpty(GuestTransferenceToken))
            return new ValidationError("GuestLoginCmd GuestTransferenceToken is empty");

        return null;
    }
}