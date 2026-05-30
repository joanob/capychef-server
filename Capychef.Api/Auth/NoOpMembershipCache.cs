using Capychef.Households.Domain.Interfaces;

namespace Capychef.Api.Auth;

public class NoOpMembershipCache : IMembershipCache
{
    public Task<bool?> GetAsync(int userId, int householdId) => Task.FromResult<bool?>(null);
    public Task SetAsync(int userId, int householdId, bool isMember) => Task.CompletedTask;
    public Task InvalidateAsync(int userId, int householdId) => Task.CompletedTask;
}

