using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;
using YourOwnBoss.Common.Entities;
using YourOwnBoss.Common.Errors;

namespace Capychef.Households.Services;

public class HouseholdInvitationService(
    CapychefDbContext dbContext,
    IHouseholdInvitationRepository invitationRepository,
    IHouseholdRepository householdRepository,
    IUserRepository userRepository) : IHouseholdInvitationService
{
    public async Task<AppError?> CreateInvitation(AuthUserDetails userDetails, CreateHouseholdInvitationCmd cmd)
    {
        var household = await householdRepository.GetTrackedHouseholdById(userDetails.HouseholdId.Value);
        if (household == null) return new NotFoundError(EntityType.Household, userDetails.HouseholdId.Value);

        var user = await userRepository.GetTrackedUserByUsernameAsync(cmd.Username);
        if (user == null) return new NotFoundError(EntityType.User, cmd.Username);

        var invitation = new HouseholdInvitation(household, user);

        await invitationRepository.AddInvitationAsync(invitation);

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<List<HouseholdInvitationDTO>> GetAllHouseholdInvitations(AuthUserDetails userDetails)
    {
        var invitations = await invitationRepository.GetHouseholdInvitations(userDetails.HouseholdId.Value);

        return HouseholdInvitationDTO.toList(invitations);
    }
}