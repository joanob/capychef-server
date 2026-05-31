using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Common.Errors;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("household-invitations")]
public class HouseholdInvitationController(
    IHouseholdInvitationService householdInvitationService,
    ILoggerFactory loggerFactory) : ControllerBase
{
    [CheckOwnership]
    [HttpPost]
    public async Task<ActionResult> CreateHouseholdInvitation(CreateHouseholdInvitationCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdInvitationService.CreateInvitation(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("HouseholdInvitationService.CreateInvitation");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [CheckOwnership]
    [HttpGet("household/{householdId}")]
    public async Task<ActionResult<ApiResponse<List<HouseholdInvitationDto>>>> GetHousholdInvitations(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        if (householdId != userDetails.GetHouseholdId())
            return BadRequest(new ApiResponse<List<HouseholdInvitationDto>>(
                new ApiError(new ValidationError("HouseholdId does not match the active household"))));

        var invitations = await householdInvitationService.GetAllHouseholdInvitations(userDetails);

        return Ok(new ApiResponse<List<HouseholdInvitationDto>>(invitations));
    }

    [CheckOwnership]
    [HttpGet("household/{householdId}/history")]
    public async Task<ActionResult<ApiResponse<List<HouseholdInvitationDto>>>> GetHouseholdInvitationsHistory(
        int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        if (householdId != userDetails.GetHouseholdId())
            return BadRequest(new ApiResponse<List<HouseholdInvitationDto>>(
                new ApiError(new ValidationError("HouseholdId does not match the active household"))));

        var invitations = await householdInvitationService.GetAllHouseholdInvitationsHistory(userDetails);

        return Ok(new ApiResponse<List<HouseholdInvitationDto>>(invitations));
    }

    [HttpGet("user")]
    public async Task<ActionResult<List<HouseholdInvitationDto>>> GetHousholdInvitationsByUser()
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

        var logger = loggerFactory.CreateLogger("HouseholdInvitationService.AcceptInvitation");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [HttpPut("reject/{invitationId}")]
    public async Task<ActionResult> RejectInvitation(int invitationId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await householdInvitationService.RejectInvitation(userDetails, invitationId);

        var logger = loggerFactory.CreateLogger("HouseholdInvitationService.RejectInvitation");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }
}