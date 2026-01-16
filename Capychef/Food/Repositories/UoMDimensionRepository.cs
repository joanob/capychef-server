using Capychef.Food.Domain.Entities;
using Capychef.Food.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Food.Repositories;

public class UoMDimensionRepository(CapychefDbContext dbContext) : IUoMDimensionRepository
{
    public async Task AddAsync(UoMDimension uomDimension)
    {
        await dbContext.UoMDimensions.AddAsync(uomDimension);
    }
}