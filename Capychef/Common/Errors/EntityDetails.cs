using Capychef.Common.Entities;

namespace Capychef.Common.Errors;

public class EntityDetails
{
    public EntityDetails(EntityType entityType)
    {
        EntityType = entityType;
    }

    public EntityDetails(EntityType entityType, int? entityId)
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public EntityDetails(EntityType entityType, string entityIdString)
    {
        EntityType = entityType;
        EntityIdString = entityIdString;
    }

    public EntityType EntityType { get; }
    public int? EntityId { get; }
    public string? EntityIdString { get; }
}