using Capychef.Common.Auth;
using Capychef.Common.Result;
using Capychef.Households.Domain.Interfaces;
using Capychef.Users.Domain.DTO;

namespace Capychef.Households.Services;

public class HouseholdMemberService(
    IHouseholdMemberRepository householdMemberRepository
) : IHouseholdMemberService
{
    public async Task<Result<List<UserDTO>>> GetHouseholdMembers(AuthUserDetails userDetails)
    {
        var members = await householdMemberRepository.GetHouseholdMembers(userDetails.HouseholdId.Value);

        return new Result<List<UserDTO>>(UserDTO.ToDTOList(members.Select(m => m.User).ToList()));
    }
}