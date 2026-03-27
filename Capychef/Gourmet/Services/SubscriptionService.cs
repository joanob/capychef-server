using Capychef.Common.Errors;
using Capychef.Gourmet.Domain.Errors;
using Capychef.Gourmet.Domain.Interfaces;
using Capychef.Households.Domain.Interfaces;

namespace Capychef.Gourmet.Services;

public class SubscriptionService(
    IUserSubscriptionRepository userSubscriptionRepository,
    IHouseholdRepository householdRepository,
    IHouseholdMemberRepository householdMemberRepository) : ISubscriptionService
{
    private readonly int _maxMembershipsFreeAccount = 2;
    private readonly int _maxOwnedHouseholdsFreeAccount = 1;

    public async Task<AppError?> CheckUserCanCreateHousehold(int userId)
    {
        var hasSubscription = await userSubscriptionRepository.UserHasActiveSubscription(userId);
        if (hasSubscription)
            return null;

        var owned = await householdRepository.CountOwnedHouseholds(userId);
        if (owned >= _maxOwnedHouseholdsFreeAccount)
            return new SubscriptionRequiredError("Max owned households reached for free account");

        var memberships = await householdMemberRepository.CountHouseholdMemberships(userId);
        if (memberships >= _maxMembershipsFreeAccount)
            return new SubscriptionRequiredError("Max household memberships reached for free account");

        return null;
    }

    public async Task<AppError?> CheckUserCanBecomeHouseholdMember(int userId)
    {
        var hasSubscription = await userSubscriptionRepository.UserHasActiveSubscription(userId);
        if (hasSubscription)
            return null;

        var memberships = await householdMemberRepository.CountHouseholdMemberships(userId);
        if (memberships >= _maxMembershipsFreeAccount)
            return new SubscriptionRequiredError("Max household memberships reached for free account");

        return null;
    }

    public async Task<bool> WillReachMembershipLimit(int userId, int additionalMemberships)
    {
        var hasSubscription = await userSubscriptionRepository.UserHasActiveSubscription(userId);
        if (hasSubscription) return false;

        var memberships = await householdMemberRepository.CountHouseholdMemberships(userId);
        return memberships + additionalMemberships >= _maxMembershipsFreeAccount;
    }
}