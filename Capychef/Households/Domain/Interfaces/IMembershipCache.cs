namespace Capychef.Households.Domain.Interfaces;

public interface IMembershipCache
{
    Task<bool?> GetAsync(int userId, int householdId);
    Task SetAsync(int userId, int householdId, bool isMember);
    Task InvalidateAsync(int userId, int householdId);

    Task<bool?> GetOwnershipAsync(int userId, int householdId);
    Task SetOwnershipAsync(int userId, int householdId, bool isOwner);
    Task InvalidateOwnershipAsync(int userId, int householdId);
}