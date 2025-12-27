using Capychef.Api.Authorization;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("household-invitations")]
public class HouseholdInvitationController(IHouseholdInvitationService householdInvitationService) : ControllerBase
{
    [CheckOwnership]
    [HttpPost]
    public async Task<ActionResult> CreateHouseholdInvitation(CreateHouseholdInvitationCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdInvitationService.CreateInvitation(userDetails, cmd);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }

    [CheckOwnership]
    [HttpGet("household")]
    public async Task<ActionResult<List<HouseholdInvitationDTO>>> GetHousholdInvitations()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var invitations = await householdInvitationService.GetAllHouseholdInvitations(userDetails);

        return Ok(invitations);
    }

    [HttpGet("user")]
    public async Task<ActionResult<List<HouseholdInvitationDTO>>> GetHousholdInvitationsByUser()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var invitations = await householdInvitationService.GetHouseholdInvitationsByUser(userDetails);

        return Ok(invitations);
    }

    [HttpPut("accept/{invitationId}")]
    public async Task<ActionResult> AcceptInvitation(int invitationId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdInvitationService.AcceptInvitation(userDetails, invitationId);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }

    [HttpPut("reject/{invitationId}")]
    public async Task<ActionResult> RejectInvitation(int invitationId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdInvitationService.RejectInvitation(userDetails, invitationId);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }
}