using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Repositories;

public class HouseholdRepository(CapychefDbContext dbContext) : IHouseholdRepository
{
    public async Task AddHouseholdAsync(Household household)
    {
        await dbContext.AddAsync(household);
    }

    public async Task<Household?> GetHouseholdByPublicIdAsync(string publicId)
    {
        return await dbContext.Households.Active().Where(x => x.PublicId == publicId).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckHouseholdOwnership(int userId, int householdId)
    {
        return await dbContext.Households.Active().Where(x => x.OwnerId == userId && x.Id == householdId).AnyAsync();
    }

    public async Task<List<Household>> GetAllHouseholds(int userId)
    {
        return await dbContext.HouseholdMembers.Active().Where(x => x.UserId == userId).Include(x => x.Household)
            .Select(x => x.Household).Active().ToListAsync();
    }

    public async Task<Household?> GetHouseholdById(int householdId)
    {
        return await dbContext.Households.AsNoTracking().Active().Where(x => x.Id == householdId).FirstOrDefaultAsync();
    }

    public async Task<Household?> GetTrackedHouseholdById(int householdId)
    {
        return await dbContext.Households.Active().Where(x => x.Id == householdId).FirstOrDefaultAsync();
    }
}