using Capychef.Common.Auth;
using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Users.Services;

public class UserSessionService(CapychefDbContext dbContext, IUserSessionRepository userSessionRepository)
    : IUserSessionService
{
    public async Task<bool> ValidateAuthUserSession(AuthUserDetails userDetails)
    {
        var userSession = await userSessionRepository.GetTrackedUserSessionByIdAsync(userDetails.SessionId);
        if (userSession == null) return false;

        if (userSession.UserId != userDetails.UserId) return false;
        if (userSession.IsRevoked) return false;

        userSession.LastRefreshAt = DateTime.Now;
        await dbContext.SaveChangesAsync();

        return true;
    }
}