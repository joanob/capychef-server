using Capychef.Common.Auth;

namespace YourOwnBoss.Game.Users.Domain.Interfaces;

public interface IUserSessionService
{
    Task<bool> ValidateAuthUserSession(AuthUserDetails userDetails);
}