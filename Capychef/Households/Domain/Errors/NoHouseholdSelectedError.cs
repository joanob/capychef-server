using YourOwnBoss.Common.Errors;

namespace Capychef.Users.Domain.Errors;

public class NoHouseholdSelectedError : AppError
{
    public NoHouseholdSelectedError(int userId) : base(ErrorType.NOT_FOUND, "")
    {
        _message = userId + " does no have a household selected";
    }
}