using YourOwnBoss.Common.Errors;

namespace Capychef.Users.Domain.Errors;

public class HouseholdOwnershipError : AppError
{
    public HouseholdOwnershipError(int userId, int houeholdId) : base(ErrorType.AUTHORIZATION, "")
    {
        _message = userId + " is not the owner of household " + houeholdId;
    }
}