using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.DTO;

public class AuthSessionDTO
{
    public AuthSessionDTO(User user, Household? activeHousehold)
    {
        User = new UserDTO(user);
        ActiveHousehold = activeHousehold != null ? new HouseholdDTO(activeHousehold) : null;
    }

    public UserDTO User { get; set; }
    public HouseholdDTO? ActiveHousehold { get; set; }
}