using Capychef.Api.Auth;
using Capychef.Api.Errors;
using Capychef.Common.Errors;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Capychef.Api.Authorization;

public class CheckMembershipFilter(
    IHouseholdService householdService,
    ILoggerFactory loggerFactory,
    IMembershipCache membershipCache)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(context.HttpContext);

        if (!userDetails.IsValid())
        {
            context.Result = new ObjectResult(new ApiResponse<object>(
                new ApiError(new AppError(ErrorType.Authentication), "")))
            {
                StatusCode = 401
            };

            return;
        }

        if (!userDetails.HasHouseholdId)
        {
            context.Result = new NotFoundResult();
            return;
        }

        var cached = await membershipCache.GetAsync(userDetails.UserId, userDetails.GetHouseholdId());

        if (cached == true)
        {
            await next();
            return;
        }

        var error = await householdService.CheckHouseholdMembership(userDetails);

        var logger = loggerFactory.CreateLogger("MembershipFilter");

        if (error == null)
        {
            await membershipCache.SetAsync(userDetails.UserId, userDetails.GetHouseholdId(), true);
            await next();
        }
        else
        {
            context.Result = GlobalErrorHandler.HandleError(error, logger);
        }
    }
}