using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.DTO;

namespace Capychef.Households.Domain.DTO;

public class HouseholdInvitationDto
{
    public HouseholdInvitationDto(HouseholdInvitation invitation)
    {
        Id = invitation.Id;

        if (invitation.User == null) throw new Exception("User can't be null in HouseholdInvitationDto.");
        if (invitation.Household == null) throw new Exception("Household can't be null in HouseholdInvitationDto.");
        if (invitation.Creator == null) throw new Exception("Creator can't be null in HouseholdInvitationDto.");

        User = new PublicUserDto(invitation.User);
        Household = new PublicHouseholdDto(invitation.Household);
        Creator = new PublicUserDto(invitation.Creator);
    }

    public int Id { get; }
    public PublicUserDto User { get; }
    public PublicHouseholdDto Household { get; }
    public PublicUserDto Creator { get; }

    public static List<HouseholdInvitationDto> ToList(List<HouseholdInvitation> invitations)
    {
        return invitations.Select(invitation => new HouseholdInvitationDto(invitation)).ToList();
    }
}