using Capychef.Common.Entities;
using Capychef.Common.Errors;

namespace Capychef.Users.Domain.Errors;

public class UsernameInUseError : AppError
{
    public UsernameInUseError(string username) : base(ErrorType.CannotCreate, EntityType.User, username)
    {
        Message = username + " already in use";
    }
}