using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Households.Domain.Cmd;

public class StorageSpaceCmd : ICmd
{
    public required string Name { get; init; }

    public required string StorageCondition { get; init; }

    public required int RowVersion { get; init; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrEmpty(Name))
            return new ValidationError("Name is required");

        if (Name.Length > 50)
            return new ValidationError("Name is too long");

        if (string.IsNullOrEmpty(StorageCondition))
            return new ValidationError("StorageCondition is required");

        if (StorageCondition is not ("A" or "R" or "F"))
            return new ValidationError("StorageCondition must be A, R or F");

        return null;
    }
}