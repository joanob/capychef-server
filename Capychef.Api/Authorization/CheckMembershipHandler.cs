using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using YourOwnBoss.Common.Auth;

namespace Capychef.Api.Authorization;

public class CheckMembershipHandler(IHttpContextAccessor httpContextAccessor, IHouseholdService householdService)
    : AuthorizationHandler<CheckMembershipRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CheckMembershipRequirement requirement)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(httpContextAccessor.HttpContext);

        if (userDetails.HouseholdId == null) return;

        var error = await householdService.CheckHouseholdMembership(userDetails);

        if (error == null) context.Succeed(requirement);
    }
}