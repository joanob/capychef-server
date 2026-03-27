using Capychef.Gourmet.Domain.Entities;
using Capychef.Gourmet.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Gourmet.Repositories;

public class UserSubscriptionRepository : IUserSubscriptionRepository
{
    private readonly CapychefDbContext _dbContext;

    public UserSubscriptionRepository(CapychefDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> UserHasActiveSubscription(int userId)
    {
        var now = DateTime.UtcNow;
        return await _dbContext.Set<Subscription>()
            .AnyAsync(s => s.UserId == userId && s.ValidFrom <= now && s.ExpiresAt > now);
    }
}