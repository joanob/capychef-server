using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("households")]
public class HouseholdController(IHouseholdService householdService, ILoggerFactory loggerFactory) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<HouseholdDTO>> CreateHousehold(CreateHouseholdCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.CreateHousehold(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("HouseholdService.CreateHousehold");

        if (household.failed()) return GlobalErrorHandler.handleError(household.error(), logger);

        userDetails.setHousehold(household.get().Id);

        JWTService.CreateAndSendJWT(userDetails, Response);

        return Ok(household.get());
    }

    [HttpGet]
    public async Task<ActionResult<List<HouseholdDTO>>> GetAllHouseholds()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var households = await householdService.GetAllHouseholds(userDetails);

        return Ok(households);
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
}