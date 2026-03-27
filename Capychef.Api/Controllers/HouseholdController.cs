using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Errors;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("households")]
public class HouseholdController(IHouseholdService householdService, ILoggerFactory loggerFactory) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<HouseholdDto>>> CreateHousehold(CreateHouseholdCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.CreateHousehold(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("HouseholdService.CreateHousehold");

        if (household.Failed()) return GlobalErrorHandler.HandleError(household.Error(), logger);

        userDetails.SetHousehold(household.Get().Id);

        JwtService.CreateAndSendJwt(userDetails, Response);

        return Ok(new ApiResponse<HouseholdDto>(household.Get()));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<HouseholdDto>>>> GetAllHouseholds()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var households = await householdService.GetAllHouseholds(userDetails);

        return Ok(new ApiResponse<List<HouseholdDto>>(households));
    }

    [HttpGet("{householdId}")]
    public async Task<ActionResult<ApiResponse<HouseholdDto>>> GetHouseholdById(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.GetHouseholdById(userDetails, householdId);

        var logger = loggerFactory.CreateLogger("HouseholdService.GetHouseholdById");

        if (household.Failed()) return HandleError(household.Error(), logger);

        return Ok(new ApiResponse<HouseholdDto>(household.Get()));
    }

    [CheckMembership]
    [HttpGet("active")]
    public async Task<ActionResult<List<HouseholdDto>>> GetActiveHousehold()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.GetActiveHousehold(userDetails);

        var logger = loggerFactory.CreateLogger("HouseholdService.GetActiveHousehold");

        if (household.Failed()) return GlobalErrorHandler.HandleError(household.Error(), logger);

        return Ok(household.Get());
    }

    [HttpGet("select/{householdId}")]
    public async Task<ActionResult<HouseholdDto>> SelectHousehold(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.SelectHousehold(userDetails, householdId);

        var logger = loggerFactory.CreateLogger("HouseholdService.SelectHousehold");

        if (household.Failed()) return GlobalErrorHandler.HandleError(household.Error(), logger);

        userDetails.SetHousehold(household.Get().Id);

        JwtService.CreateAndSendJwt(userDetails, Response);

        return Ok(household.Get());
    }

    [CheckOwnership]
    [HttpPut("{householdId}")]
    public async Task<ActionResult<HouseholdDto>> UpdateHousehold(int householdId, HouseholdCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.UpdateHousehold(userDetails, householdId, cmd);

        var logger = loggerFactory.CreateLogger("HouseholdService.UpdateHousehold");

        if (household.Failed()) return GlobalErrorHandler.HandleError(household.Error(), logger);

        return Ok(household.Get());
    }

    [CheckOwnership]
    [HttpDelete("{householdId}")]
    public async Task<ActionResult> DeleteHousehold(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdService.DeleteHousehold(userDetails);

        var logger = loggerFactory.CreateLogger("HouseholdService.DeleteHousehold");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        JwtService.CreateAndSendJwt(new AuthUserDetails(userDetails.UserId, userDetails.SessionId), Response);

        return Ok();
    }

    private ActionResult HandleError(AppError error, ILogger logger)
    {
        if (error is HouseholdMembershipError householdMembershipError)
        {
            logger.LogError(householdMembershipError.Message);

            if (householdMembershipError.Entity is { EntityId: not null })
                return new ObjectResult(new ApiResponse<HouseholdDto>(new ApiError(new NotFoundError(
                    EntityType.Household,
                    householdMembershipError.Entity.EntityId.Value))))
                {
                    StatusCode = 404
                };
        }

        return GlobalErrorHandler.HandleError(error, logger);
    }
}