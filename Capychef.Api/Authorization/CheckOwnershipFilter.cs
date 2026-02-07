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
            context.Result = new ObjectResult(new ApiResponse<HouseholdDTO>(
                new ApiError(new NoHouseholdSelectedError(userDetails.UserId), "NO_HOUSEHOLD_SELECTED")))
            {
                StatusCode = 404
            };

            return;
        }

        var error = await householdService.CheckHouseholdOwnership(userDetails);

        if (error == null)
            await next();
        else
            context.Result = new ObjectResult(new ApiResponse<HouseholdDTO>(new ApiError(new NotFoundError(
                EntityType.Household,
                userDetails.HouseholdId.Value))))
            {
                StatusCode = 404
            };
    }
}