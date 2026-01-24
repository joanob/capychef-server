using Capychef.Storage.Domain.Entities;

namespace Capychef.Storage.Domain.Interfaces;

public interface IBatchRepository
{
    Task AddAsync(Batch batch);
}