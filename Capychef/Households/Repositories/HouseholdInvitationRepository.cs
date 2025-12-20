using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Households.Repositories;

public class HouseholdInvitationRepository(CapychefDbContext dbContext) : IHouseholdInvitationRepository
{
    public async Task AddInvitationAsync(HouseholdInvitation invitation)
    {
        await dbContext.HouseholdInvitations.AddAsync(invitation);
    }
}