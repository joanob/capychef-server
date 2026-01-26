using Capychef.Common.Errors;

namespace Capychef.Households.Domain.Errors;

public class NoHouseholdSelectedError : AppError
{
    public NoHouseholdSelectedError(int userId) : base(ErrorType.NOT_FOUND, "")
    {
        _message = userId + " does no have a household selected";
    }
}