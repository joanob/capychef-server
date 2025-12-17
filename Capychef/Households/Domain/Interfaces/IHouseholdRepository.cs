using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdRepository
{
    Task AddHouseholdAsync(Household household);
    Task<Household?> GetHouseholdByPublicIdAsync(string publicId);
}