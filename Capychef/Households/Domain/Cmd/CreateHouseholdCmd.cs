using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Households.Domain.Cmd;

public class CreateHouseholdCmd : ICmd
{
    public required string Name { get; init; }
    public required List<int> InitialStorageSpaceIds { get; init; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrWhiteSpace(Name)) return new ValidationError("Name is required");

        if (Name.Length > 50) return new ValidationError("Name is too long");

        return null;
    }
}