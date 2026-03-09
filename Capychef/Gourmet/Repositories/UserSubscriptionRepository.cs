using Capychef.Common.Auth;
using Capychef.Gourmet.Domain.Entities;
using Capychef.Gourmet.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

public class UserSubscriptionRepository : IUserSubscriptionRepository
{
    private readonly CapychefDbContext _dbContext;

    public UserSubscriptionRepository(CapychefDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> UserHasActiveSubscription(AuthUserDetails user)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.Set<Subscription>()
            .AnyAsync(s => s.UserId == user.UserId && s.ValidFrom <= now && s.ExpiresAt > now);
    }
}