using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdInvitationRepository
{
    Task AddInvitationAsync(HouseholdInvitation invitation);
}