using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Common.Auth;
using Capychef.Households.Domain.Interfaces;
using Capychef.Users.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("household-members")]
public class HouseholdMemberController(IHouseholdMemberService householdMemberService, ILoggerFactory loggerFactory)
    : ControllerBase
{
    [CheckMembership]
    [HttpGet("{householdId}")]
    public async Task<ActionResult<ApiResponse<List<UserDTO>>>> GetHouseholdMembers(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var members = await householdMemberService.GetHouseholdMembers(userDetails);

        var logger = loggerFactory.CreateLogger("HouseholdService.GetHouseholdMembers");

        if (members.failed()) return GlobalErrorHandler.handleError(members.error(), logger);

        return Ok(new ApiResponse<List<UserDTO>>(members.get()));
    }

    [CheckMembership]
    [HttpDelete("{householdId}")]
    public async Task<ActionResult> LeaveHousehold(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdMemberService.LeaveHousehold(userDetails);

        var logger = loggerFactory.CreateLogger("HouseholdService.LeaveHousehold");

        if (error != null) return GlobalErrorHandler.handleError(error, logger);

        JWTService.CreateAndSendJWT(new AuthUserDetails(userDetails.UserId, userDetails.SessionId), Response);

        return Ok();
    }
}