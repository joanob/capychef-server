using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.DTO;

namespace Capychef.Households.Domain.DTO;

public class HouseholdJoinRequestDto
{
    public HouseholdJoinRequestDto(HouseholdJoinRequest joinRequest)
    {
        Id = joinRequest.Id;
        if (joinRequest.Household == null) throw new Exception("Household can't be null in HouseholdJoinRequestDto.");
        if (joinRequest.User == null) throw new Exception("User can't be null in HouseholdJoinRequestDto.");
        Household = new PublicHouseholdDto(joinRequest.Household);
        User = new PublicUserDto(joinRequest.User);
    }

    public int Id { get; set; }
    public PublicUserDto User { get; set; }
    public PublicHouseholdDto Household { get; set; }

    public static List<HouseholdJoinRequestDto> ToList(List<HouseholdJoinRequest> invitations)
    {
        return invitations.Select(invitation => new HouseholdJoinRequestDto(invitation)).ToList();
    }
}