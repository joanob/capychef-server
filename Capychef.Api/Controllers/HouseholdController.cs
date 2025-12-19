using Capychef.Api.Authorization;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("households")]
public class HouseholdController(IHouseholdService householdService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<HouseholdDTO>> CreateHousehold(CreateHouseholdCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.CreateHousehold(userDetails, cmd);

        if (household.failed()) return GlobalErrorHandler.handleError(household.error());

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

    [HttpGet("select/{householdId}")]
    public async Task<ActionResult<HouseholdDTO>> SelectHousehold(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var household = await householdService.SelectHousehold(userDetails, householdId);

        if (household.failed()) return GlobalErrorHandler.handleError(household.error());

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

        if (household.failed()) return GlobalErrorHandler.handleError(household.error());

        return Ok(household.get());
    }
}