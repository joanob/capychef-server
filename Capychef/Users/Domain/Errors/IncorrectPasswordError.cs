using Capychef.Common.Entities;
using Capychef.Common.Errors;

namespace Capychef.Users.Domain.Errors;

public class IncorrectPasswordError : AppError
{
    public IncorrectPasswordError(int userId) : base(ErrorType.Authorization, EntityType.User, userId)
    {
        Message = "incorrect password for user " + userId;
    }
}