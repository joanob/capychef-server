using Capychef.Common.Auth;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserSessionService
{
    Task<bool> ValidateAuthUserSession(AuthUserDetails userDetails);
}