using Capychef.Common.Errors;

namespace Capychef.Households.Domain.Errors;

public class HouseholdMembershipError : AppError
{
    public HouseholdMembershipError(int userId, int houeholdId) : base(ErrorType.AUTHORIZATION, "")
    {
        _message = userId + " is not a member of household " + houeholdId;
    }
}