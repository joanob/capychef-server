using Capychef.Api.Authorization;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("household-join-requests")]
public class HouseholdJoinRequestController(IHouseholdJoinRequestService householdJoinRequestService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateHouseholdJoinRequest(CreateHouseholdJoinRequestCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdJoinRequestService.CreateJoinRequest(userDetails, cmd);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }

    [CheckOwnership]
    [HttpGet("household")]
    public async Task<ActionResult<List<HouseholdJoinRequestDTO>>> GetHouseholdJoinRequests()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var joinRequests = await householdJoinRequestService.GetAllHouseholdJoinRequests(userDetails);

        return Ok(joinRequests);
    }

    [HttpGet("user")]
    public async Task<ActionResult<List<HouseholdJoinRequestDTO>>> GetHousholdJoinRequestsByUser()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var joinRequests = await householdJoinRequestService.GetHouseholdJoinRequestsByUser(userDetails);

        return Ok(joinRequests);
    }

    [HttpPut("accept/{joinRequestId}")]
    public async Task<ActionResult> AcceptJoinRequest(int joinRequestId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdJoinRequestService.AcceptJoinRequest(userDetails, joinRequestId);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }

    [HttpPut("reject/{joinRequestId}")]
    public async Task<ActionResult> RejectJoinRequest(int joinRequestId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdJoinRequestService.RejectJoinRequest(userDetails, joinRequestId);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }
}