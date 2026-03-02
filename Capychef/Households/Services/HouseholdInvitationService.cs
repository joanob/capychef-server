using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Errors;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Households.Services;

public class HouseholdInvitationService(
    CapychefDbContext dbContext,
    IHouseholdInvitationRepository invitationRepository,
    IHouseholdMemberRepository householdMemberRepository,
    IHouseholdRepository householdRepository,
    IUserRepository userRepository, IHouseholdInvitationRealtimeService householdInvitationRealtimeService) : IHouseholdInvitationService
{
    public async Task<AppError?> CreateInvitation(AuthUserDetails userDetails, CreateHouseholdInvitationCmd cmd)
    {
        var household = await householdRepository.GetTrackedHouseholdById(userDetails.HouseholdId.Value);
        if (household == null) return new NotFoundError(EntityType.Household, userDetails.HouseholdId.Value);

        var user = await userRepository.GetTrackedUserByUsernameAsync(cmd.Username);
        if (user == null) return new NotFoundError(EntityType.User, cmd.Username);

        if (await invitationRepository.CheckNonAnsweredInvitationExistsByHouseholdIdAndUserId(household.Id, user.Id))
        {
            return new UserHasUnansweredHouseholdInvitation(user.Id, household.Id);
        }
        
        var invitation = new HouseholdInvitation(household, user);

        await invitationRepository.AddInvitationAsync(invitation);

        await dbContext.SaveChangesAsync();
        
        householdInvitationRealtimeService.SendHouseholdInvitationReceivedMessage(invitation);

        return null;
    }

    public async Task<List<HouseholdInvitationDTO>> GetAllHouseholdInvitations(AuthUserDetails userDetails)
    {
        var invitations = await invitationRepository.GetHouseholdInvitations(userDetails.HouseholdId.Value);

        return HouseholdInvitationDTO.toList(invitations);
    }

    public async Task<List<HouseholdInvitationDTO>> GetHouseholdInvitationsByUser(AuthUserDetails userDetails)
    {
        var invitations = await invitationRepository.GetHouseholdInvitationsByUserId(userDetails.UserId);

        return HouseholdInvitationDTO.toList(invitations);
    }

    public async Task<AppError?> AcceptInvitation(AuthUserDetails userDetails, int invitationId)
    {
        var invitation = await invitationRepository.GetTrackedInvitationById(invitationId, userDetails.UserId);

        if (invitation == null || invitation.IsAnswered)
            return new NotFoundError(EntityType.HouseholdInvitation, userDetails.UserId);

        invitation.IsAnswered = true;
        invitation.AnsweredAt = DateTime.Now;
        invitation.IsAccepted = true;

        var member = new HouseholdMember(invitation.HouseholdId, invitation.UserId);

        await householdMemberRepository.AddHouseholdMemberAsync(member);

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> RejectInvitation(AuthUserDetails userDetails, int invitationId)
    {
        var invitation = await invitationRepository.GetTrackedInvitationById(invitationId, userDetails.UserId);

        if (invitation == null || invitation.IsAnswered)
            return new NotFoundError(EntityType.HouseholdInvitation, userDetails.UserId);

        invitation.IsAnswered = true;
        invitation.AnsweredAt = DateTime.Now;
        invitation.IsAccepted = false;

        await dbContext.SaveChangesAsync();

        return null;
    }
}