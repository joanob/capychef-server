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
}