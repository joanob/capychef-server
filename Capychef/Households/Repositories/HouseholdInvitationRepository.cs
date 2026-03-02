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
        return await dbContext.HouseholdInvitations.Active().AsNoTracking().Where(x => x.HouseholdId == householdId)
            .Include(x => x.User).ToListAsync();
    }

    public async Task<List<HouseholdInvitation>> GetHouseholdInvitationsByUserId(int userId)
    {
        return await dbContext.HouseholdInvitations.Active().Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<HouseholdInvitation?> GetTrackedInvitationById(int invitationId, int userId)
    {
        return await dbContext.HouseholdInvitations.Active().Where(x => x.Id == invitationId && x.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CheckNonAnsweredInvitationExistsByHouseholdIdAndUserId(int householdId, int userId)
    {
        return await dbContext.HouseholdInvitations.AsNoTracking().Active()
            .Where(x => x.HouseholdId == householdId && x.UserId == userId && !x.IsAnswered).AnyAsync();
    }
}