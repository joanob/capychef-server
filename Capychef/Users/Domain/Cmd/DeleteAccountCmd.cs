using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Users.Domain.Cmd;

public class DeleteAccountCmd : ICmd
{
    public string? Password { get; set; }

    public ValidationError? Validate()
    {
        return null;
    }
}