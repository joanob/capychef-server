using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Gourmet.Domain.Interfaces;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Errors;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Services;

public class HouseholdInvitationService(
    CapychefDbContext dbContext,
    IHouseholdInvitationRepository invitationRepository,
    IHouseholdMemberRepository householdMemberRepository,
    IHouseholdRepository householdRepository,
    IUserRepository userRepository,
    ISubscriptionService subscriptionService,
    IHouseholdInvitationRealtimeService householdInvitationRealtimeService,
    IMembershipCache membershipCache,
    IHouseholdRealtimeService householdRealtimeService) : IHouseholdInvitationService
{
    public async Task<AppError?> CreateInvitation(AuthUserDetails userDetails, CreateHouseholdInvitationCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return validationError;

        var household = await householdRepository.GetTrackedHouseholdById(userDetails.GetHouseholdId());
        if (household == null) return new NotFoundError(EntityType.Household, userDetails.GetHouseholdId());

        var user = await userRepository.GetTrackedUserByUsernameAsync(cmd.Username);
        if (user == null) return new NotFoundError(EntityType.User, cmd.Username);

        if (await householdMemberRepository.CheckHouseholdMembership(user.Id, household.Id))
            return new ValidationError("User is already a member of this household");

        if (await invitationRepository.CheckNonAnsweredInvitationExistsByHouseholdIdAndUserId(household.Id, user.Id))
            return new UserHasUnansweredHouseholdInvitation(user.Id, household.Id);

        var invitation = new HouseholdInvitation(household, user, userDetails.UserId);

        await invitationRepository.AddInvitationAsync(invitation);

        await dbContext.SaveChangesAsync();

        _ = householdInvitationRealtimeService.SendHouseholdInvitationReceivedMessage(invitation);

        return null;
    }

    public async Task<List<HouseholdInvitationDto>> GetAllHouseholdInvitations(AuthUserDetails userDetails)
    {
        var invitations = await invitationRepository.GetHouseholdInvitations(userDetails.GetHouseholdId());
        return HouseholdInvitationDto.ToList(invitations);
    }

    public async Task<List<HouseholdInvitationDto>> GetAllHouseholdInvitationsHistory(AuthUserDetails userDetails)
    {
        var invitations = await invitationRepository.GetAllHouseholdInvitationsHistory(userDetails.GetHouseholdId());
        return HouseholdInvitationDto.ToList(invitations);
    }

    public async Task<List<HouseholdInvitationDto>> GetHouseholdInvitationsByUser(AuthUserDetails userDetails)
    {
        var invitations = await invitationRepository.GetHouseholdInvitationsByUserId(userDetails.UserId);

        return HouseholdInvitationDto.ToList(invitations);
    }

    public async Task<AppError?> AcceptInvitation(AuthUserDetails userDetails, int invitationId)
    {
        var invitation = await invitationRepository.GetTrackedInvitationById(invitationId, userDetails.UserId);

        if (invitation == null || invitation.IsAnswered)
            return new NotFoundError(EntityType.HouseholdInvitation, userDetails.UserId);

        var error = await subscriptionService.CheckUserCanBecomeHouseholdMember(invitation.UserId);
        if (error != null) return error;

        invitation.Accept();

        var member = new HouseholdMember(invitation.HouseholdId, invitation.UserId);

        await householdMemberRepository.AddHouseholdMemberAsync(member);

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ConcurrencyError();
        }

        await membershipCache.InvalidateAsync(invitation.UserId, invitation.HouseholdId);

        _ = householdRealtimeService.SendMemberJoinedMessage(invitation.HouseholdId, invitation.UserId);

        return null;
    }

    public async Task<AppError?> RejectInvitation(AuthUserDetails userDetails, int invitationId)
    {
        var invitation = await invitationRepository.GetTrackedInvitationById(invitationId, userDetails.UserId);

        if (invitation == null || invitation.IsAnswered)
            return new NotFoundError(EntityType.HouseholdInvitation, userDetails.UserId);

        invitation.Reject();

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ConcurrencyError();
        }

        return null;
    }
}