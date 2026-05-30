using Capychef.Api.Auth;
using Capychef.Api.Errors;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Errors;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Capychef.Api.Authorization;

public class CheckOwnershipFilter(
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
            context.Result = new ObjectResult(new ApiResponse<HouseholdDto>(
                new ApiError(new NoHouseholdSelectedError(userDetails.UserId), "NO_HOUSEHOLD_SELECTED")))
            {
                StatusCode = 404
            };

            return;
        }

        var cached = await membershipCache.GetOwnershipAsync(userDetails.UserId, userDetails.GetHouseholdId());

        if (cached == true)
        {
            await next();
            return;
        }

        var error = await householdService.CheckHouseholdOwnership(userDetails);

        var logger = loggerFactory.CreateLogger("OwnershipFilter");

        if (error == null)
        {
            await membershipCache.SetOwnershipAsync(userDetails.UserId, userDetails.GetHouseholdId(), true);
            await next();
        }
        else
        {
            logger.LogWarning(error.Message);

            context.Result = new ObjectResult(new ApiResponse<HouseholdDto>(new ApiError(new NotFoundError(
                EntityType.Household,
                userDetails.GetHouseholdId()))))
            {
                StatusCode = 404
            };
        }
    }
}