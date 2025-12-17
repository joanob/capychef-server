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

        return Ok(household.get());
    }
}