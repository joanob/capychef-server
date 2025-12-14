using YourOwnBoss.Common.Errors;

namespace Capychef.Users.Domain.Errors;

public class UsernameInUseError : AppError
{
    public UsernameInUseError(string username) : base(ErrorType.CANNOT_CREATE, "")
    {
        _message = username + " already in use";
    }
}