using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.DTO;

public class AuthSessionDto
{
    public AuthSessionDto(User user, Household? activeHousehold)
    {
        User = new UserDto(user);
        ActiveHousehold = activeHousehold != null ? new HouseholdDto(activeHousehold) : null;
    }

    public UserDto User { get; set; }
    public HouseholdDto? ActiveHousehold { get; set; }
}