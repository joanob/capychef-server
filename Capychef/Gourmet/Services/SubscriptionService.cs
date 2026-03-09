using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Gourmet.Domain.Errors;
using Capychef.Gourmet.Domain.Interfaces;
using Capychef.Households.Domain.Interfaces;

namespace Capychef.Gourmet.Domain.Services;

public class SubscriptionService(
    IUserSubscriptionRepository userSubscriptionRepository,
    IHouseholdRepository householdRepository,
    IHouseholdMemberRepository householdMemberRepository) : ISubscriptionService
{
    private readonly int MAX_MEMBERSHIPS_FREE_ACCOUNT = 2;
    private readonly int MAX_OWNED_HOUSEHOLDS_FREE_ACCOUNT = 1;

    public async Task<AppError?> CheckUserCanCreateHousehold(AuthUserDetails userDetails)
    {
        var hasSubscription = await userSubscriptionRepository.UserHasActiveSubscription(userDetails);
        if (hasSubscription)
            return null;

        var owned = await householdRepository.CountOwnedHouseholds(userDetails.UserId);
        if (owned >= MAX_OWNED_HOUSEHOLDS_FREE_ACCOUNT)
            return new SubscriptionRequiredError("Max owned households reached for free account");

        var memberships = await householdMemberRepository.CountHouseholdMemberships(userDetails.UserId);
        if (memberships >= MAX_MEMBERSHIPS_FREE_ACCOUNT)
            return new SubscriptionRequiredError("Max household memberships reached for free account");

        return null;
    }

    public async Task<AppError?> CheckUserCanBecomeHouseholdMember(AuthUserDetails userDetails)
    {
        var hasSubscription = await userSubscriptionRepository.UserHasActiveSubscription(userDetails);
        if (hasSubscription)
            return null;

        var memberships = await householdMemberRepository.CountHouseholdMemberships(userDetails.UserId);
        if (memberships >= MAX_MEMBERSHIPS_FREE_ACCOUNT)
            return new SubscriptionRequiredError("Max household memberships reached for free account");

        return null;
    }
}