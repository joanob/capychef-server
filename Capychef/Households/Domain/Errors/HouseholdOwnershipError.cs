using Capychef.Common.Errors;

namespace Capychef.Households.Domain.Errors;

public class HouseholdOwnershipError : AppError
{
    public HouseholdOwnershipError(int userId, int houeholdId) : base(ErrorType.Authorization)
    {
        Message = userId + " is not the owner of household " + houeholdId;
    }
}