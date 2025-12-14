using Capychef.Common.Auth;
using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Interfaces;
using YourOwnBoss.Game.Users.Domain.Interfaces;

namespace YourOwnBoss.Game.Users.Services;

public class UserSessionService(CapychefDbContext dbContext, IUserSessionRepository userSessionRepository)
    : IUserSessionService
{
    public async Task<bool> ValidateAuthUserSession(AuthUserDetails userDetails)
    {
        var userSession = await userSessionRepository.GetTrackedUserSessionByIdAsync(userDetails.SessionId);
        if (userSession == null) return false;

        if (userSession.UserId != userDetails.UserId) return false;
        if (userSession.IsRevoked) return false;

        userSession.LastConnectionAt = DateTime.Now;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<UserSession> CreateUserSession(User user)
    {
        var userSession = new UserSession(user);
        await userSessionRepository.AddUserSessionAsync(userSession);
        return userSession;
    }
}