using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Repositories;

public class HouseholdInvitationRepository(CapychefDbContext dbContext) : IHouseholdInvitationRepository
{
    public async Task AddInvitationAsync(HouseholdInvitation invitation)
    {
        await dbContext.HouseholdInvitations.AddAsync(invitation);
    }

    public async Task<List<HouseholdInvitation>> GetHouseholdInvitations(int householdId)
    {
        return await dbContext.HouseholdInvitations.Active().Where(x => x.HouseholdId == householdId).ToListAsync();
    }
}