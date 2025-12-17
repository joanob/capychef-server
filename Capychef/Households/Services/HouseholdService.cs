using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;
using YourOwnBoss.Common.Result;

namespace Capychef.Households.Services;

public class HouseholdService(
    CapychefDbContext dbContext,
    IHouseholdRepository householdRepository,
    IUserRepository userRepository) : IHouseholdService
{
    public async Task<Result<HouseholdDTO>> CreateHousehold(AuthUserDetails userDetails, CreateHouseholdCmd cmd)
    {
        var household = new Household(userDetails.UserId, cmd.Name);

        await householdRepository.AddHouseholdAsync(household);

        await dbContext.SaveChangesAsync();

        return new Result<HouseholdDTO>(new HouseholdDTO(household));
    }
}