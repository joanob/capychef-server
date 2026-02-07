using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
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
    public async Task<ActionResult<ApiResponse<HouseholdDTO>>> CreateHousehold(CreateHouseholdCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.CreateHousehold(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("HouseholdService.CreateHousehold");

        if (household.failed()) return GlobalErrorHandler.handleError(household.error(), logger);

        userDetails.setHousehold(household.get().Id);

        JWTService.CreateAndSendJWT(userDetails, Response);

        return Ok(new ApiResponse<HouseholdDTO>(household.get()));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<HouseholdDTO>>>> GetAllHouseholds()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var households = await householdService.GetAllHouseholds(userDetails);

        return Ok(new ApiResponse<List<HouseholdDTO>>(households));
    }

    [HttpGet("{householdId}")]
    public async Task<ActionResult<ApiResponse<HouseholdDTO>>> GetHouseholdById(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.GetHouseholdById(userDetails, householdId);

        var logger = loggerFactory.CreateLogger("HouseholdService.GetHouseholdById");

        if (household.failed()) return handleError(household.error(), logger);

        return Ok(new ApiResponse<HouseholdDTO>(household.get()));
    }

    [CheckMembership]
    [HttpGet("active")]
    public async Task<ActionResult<List<HouseholdDTO>>> GetActiveHousehold()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.GetActiveHousehold(userDetails);

        var logger = loggerFactory.CreateLogger("HouseholdService.GetActiveHousehold");

        if (household.failed()) return GlobalErrorHandler.handleError(household.error(), logger);

        return Ok(household.get());
    }

    [HttpGet("select/{householdId}")]
    public async Task<ActionResult<HouseholdDTO>> SelectHousehold(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.SelectHousehold(userDetails, householdId);

        var logger = loggerFactory.CreateLogger("HouseholdService.SelectHousehold");

        if (household.failed()) return GlobalErrorHandler.handleError(household.error(), logger);

        userDetails.setHousehold(household.get().Id);

        JWTService.CreateAndSendJWT(userDetails, Response);

        return Ok(household.get());
    }

    [CheckOwnership]
    [HttpPut("{householdId}")]
    public async Task<ActionResult<HouseholdDTO>> UpdateHousehold(int householdId, UpdateHouseholdCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.UpdateHousehold(userDetails, householdId, cmd);

        var logger = loggerFactory.CreateLogger("HouseholdService.UpdateHousehold");

        if (household.failed()) return GlobalErrorHandler.handleError(household.error(), logger);

        return Ok(household.get());
    }

    private ActionResult handleError(AppError error, ILogger logger)
    {
        if (error is HouseholdMembershipError householdMembershipError)
        {
            logger.LogError(householdMembershipError.Message);

            if (householdMembershipError.Entity.EntityId.HasValue)
                return new ObjectResult(new ApiResponse<HouseholdDTO>(new ApiError(new NotFoundError(
                    EntityType.Household,
                    householdMembershipError.Entity.EntityId.Value))))
                {
                    StatusCode = 404
                };
        }

        return GlobalErrorHandler.handleError(error, logger);
    }
}