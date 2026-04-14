using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Shopping.Domain.Cmd;

public class SupermarketCmd : ICmd
{
    public required string Name { get; init; }

    public ValidationError? Validate()
    {
        if (Name.Length > 50) return new ValidationError("Name cannot be longer than 50 characters.");

        return null;
    }
}