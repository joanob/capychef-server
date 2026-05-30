using Capychef.Common.Auth;
using Capychef.Common.Errors;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserSessionService
{
    Task<bool> ValidateAuthUserSession(AuthUserDetails userDetails);
    Task<AppError?> Logout(AuthUserDetails userDetails);
}