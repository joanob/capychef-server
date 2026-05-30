using Capychef.Common.Auth;
using Capychef.Common.Errors;
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

        userSession.LastRefreshAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<AppError?> Logout(AuthUserDetails userDetails)
    {
        var session = await userSessionRepository.GetTrackedUserSessionByIdAsync(userDetails.SessionId);

        if (session != null && !session.IsRevoked)
        {
            session.IsRevoked = true;
            await dbContext.SaveChangesAsync();
        }

        return null;
    }
}