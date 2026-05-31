using Capychef.Common.Errors;

namespace Capychef.Households.Domain.Errors;

public class HouseholdMembershipError : AppError
{
    public HouseholdMembershipError(int userId, int householdId) : base(ErrorType.Authorization)
    {
        Message = userId + " is not a member of household " + householdId;
    }
}