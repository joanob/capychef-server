using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Households.Domain.Cmd;

public class CreateHouseholdInvitationCmd : ICmd
{
    public required string Username { get; init; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrWhiteSpace(Username)) return new ValidationError("Username is required");

        if (Username.Length > 40) return new ValidationError("Username must be less than 40 characters");

        return null;
    }
}