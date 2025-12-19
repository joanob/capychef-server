namespace Capychef.Api.Authorization;

public static class AuhorizationPolicies
{
    public static readonly string HouseholdOwnership = "HouseholdOwnerShip";
    public static readonly string HouseholdMembership = "HouseholdMembership";

    public static IServiceCollection RegisterAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
            options.AddPolicy(HouseholdOwnership, policy => policy.Requirements.Add(new CheckOwnershipRequirement())));

        services.AddAuthorization(options => options.AddPolicy(HouseholdMembership,
            policy => policy.Requirements.Add(new CheckMembershipRequirement())));

        return services;
    }
}