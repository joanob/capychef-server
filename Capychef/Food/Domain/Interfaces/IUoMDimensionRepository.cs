using Capychef.Food.Domain.Entities;

namespace Capychef.Food.Domain.Interfaces;

public interface IUoMDimensionRepository
{
    Task AddAsync(UoMDimension uomDimension);
}