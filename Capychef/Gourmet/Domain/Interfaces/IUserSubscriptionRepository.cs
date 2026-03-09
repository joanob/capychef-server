namespace Capychef.Gourmet.Domain.Interfaces;

public interface IUserSubscriptionRepository
{
    Task<bool> UserHasActiveSubscription(int userId);
}