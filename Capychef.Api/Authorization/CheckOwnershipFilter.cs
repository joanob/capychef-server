using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YourOwnBoss.Common.Auth;

namespace Capychef.Api.Authorization;

public class CheckOwnershipFilter(IHouseholdService householdService)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(context.HttpContext);

        if (userDetails.HouseholdId == null)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var error = await householdService.CheckHouseholdOwnership(userDetails);

        if (error == null) await next();
    }
}