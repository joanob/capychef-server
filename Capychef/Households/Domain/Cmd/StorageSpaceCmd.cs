using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Households.Domain.Cmd;

public class StorageSpaceCmd : ICmd
{
    public required string Name { get; init; }

    public required string StorageCondition { get; init; }

    public ValidationError? Validate()
    {
        if (string.IsNullOrEmpty(Name))
            return new ValidationError("Name is required");

        if (string.IsNullOrEmpty(StorageCondition))
            return new ValidationError("StorageCondition is required");

        return null;
    }
}