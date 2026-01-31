using Capychef.Common.Errors;

namespace Capychef.Households.Domain.Errors;

public class NoHouseholdSelectedError : AppError
{
    public NoHouseholdSelectedError(int userId) : base(ErrorType.NotFound)
    {
        Message = userId + " does no have a household selected";
    }
}