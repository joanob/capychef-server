using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Households.Domain.Cmd;

public class CreateHouseholdJoinRequestCmd : ICmd
{
    public required string HouseholdPublicId { get; init; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrWhiteSpace(HouseholdPublicId)) return new ValidationError("HouseholdPublicId is required");

        return null;
    }
}