using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Repositories;

public class HouseholdMemberRepository(CapychefDbContext dbContext) : IHouseholdMemberRepository
{
    public async Task AddHouseholdMemberAsync(HouseholdMember member)
    {
        await dbContext.HouseholdMembers.AddAsync(member);
    }

    public async Task<bool> CheckHouseholdMembership(int userId, int householdId)
    {
        return await dbContext.HouseholdMembers.Active().Where(x => x.UserId == userId && x.HouseholdId == householdId)
            .AnyAsync();
    }

    public async Task<List<HouseholdMember>> GetHouseholdMembers(int householdId)
    {
        return await dbContext.HouseholdMembers.Active().AsNoTracking().Where(x => x.HouseholdId == householdId)
            .Include(x => x.User).ToListAsync();
    }
}