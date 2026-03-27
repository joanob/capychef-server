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
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetHouseholdMembers(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var members = await householdMemberService.GetHouseholdMembers(userDetails);

        var logger = loggerFactory.CreateLogger("HouseholdService.GetHouseholdMembers");

        if (members.Failed()) return GlobalErrorHandler.HandleError(members.Error(), logger);

        return Ok(new ApiResponse<List<UserDto>>(members.Get()));
    }

    [CheckMembership]
    [HttpDelete("leave/{householdId}")]
    public async Task<ActionResult> LeaveHousehold(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdMemberService.LeaveHousehold(userDetails);

        var logger = loggerFactory.CreateLogger("HouseholdService.LeaveHousehold");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        JwtService.CreateAndSendJwt(new AuthUserDetails(userDetails.UserId, userDetails.SessionId), Response);

        return Ok();
    }

    [CheckOwnership]
    [HttpDelete("remove/{householdMemberId}")]
    public async Task<ActionResult> RemoveMember(int householdMemberId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdMemberService.RemoveMember(userDetails, householdMemberId);

        var logger = loggerFactory.CreateLogger("HouseholdService.LeaveHousehold");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }
}