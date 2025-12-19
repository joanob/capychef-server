using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using YourOwnBoss.Common.Auth;

namespace Capychef.Api.Authorization;

public class CheckOwnershipHandler(IHttpContextAccessor httpContextAccessor, IHouseholdService householdService)
    : AuthorizationHandler<CheckOwnershipRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CheckOwnershipRequirement requirement)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(httpContextAccessor.HttpContext);

        if (userDetails.HouseholdId == null) return;

        var error = await householdService.CheckHouseholdOwnership(userDetails);

        if (error == null) context.Succeed(requirement);
    }
}