using Capychef.Common.Auth;

namespace Capychef.Gourmet.Domain.Interfaces;

public interface IUserSubscriptionRepository
{
    Task<bool> UserHasActiveSubscription(AuthUserDetails user);
}