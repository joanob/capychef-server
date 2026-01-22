using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Authorization;

public class CheckMembershipFilter(IHouseholdService householdService, ILoggerFactory loggerFactory)
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

        var error = await householdService.CheckHouseholdMembership(userDetails);

        var logger = loggerFactory.CreateLogger("MembershipFilter");

        if (error == null)
            await next();
        else
            GlobalErrorHandler.handleError(error, logger);
    }
}