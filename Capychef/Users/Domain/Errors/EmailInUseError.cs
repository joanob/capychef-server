using Capychef.Common.Entities;
using Capychef.Common.Errors;

namespace Capychef.Users.Domain.Errors;

public class EmailInUseError : AppError
{
    public EmailInUseError(string email) : base(ErrorType.CannotCreate, EntityType.User, email)
    {
        Message = "email " + email + " already in use";
    }
}