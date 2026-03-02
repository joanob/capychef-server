using Capychef.Common.Errors;

namespace Capychef.Households.Domain.Errors;

public class UserHasUnansweredHouseholdInvitation : AppError
{
    public UserHasUnansweredHouseholdInvitation(int userId, int householdId) : base(ErrorType.CannotCreate)
    {
        Message = userId + " has an unanswered invitation to " + householdId;
    }
}