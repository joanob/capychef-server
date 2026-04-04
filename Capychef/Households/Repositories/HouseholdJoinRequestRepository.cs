using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Repositories;

public class HouseholdJoinRequestRepository(CapychefDbContext dbContext) : IHouseholdJoinRequestRepository
{
    public async Task AddJoinRequestAsync(HouseholdJoinRequest invitation)
    {
        await dbContext.HouseholdJoinRequests.AddAsync(invitation);
    }

    public async Task<List<HouseholdJoinRequest>> GetHouseholdJoinRequests(int householdId)
    {
        return await dbContext.HouseholdJoinRequests.Active().Where(x => x.HouseholdId == householdId).ToListAsync();
    }

    public async Task<List<HouseholdJoinRequest>> GetHouseholdJoinRequestsByUserId(int userId)
    {
        return await dbContext.HouseholdJoinRequests.Active().Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<HouseholdJoinRequest?> GetTrackedJoinRequestById(int joinRequestId, int householdId)
    {
        return await dbContext.HouseholdJoinRequests.Active()
            .Where(x => x.Id == joinRequestId && x.HouseholdId == householdId)
            .FirstOrDefaultAsync();
    }
}