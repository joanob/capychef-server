using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Common.Entities;

public class BaseDeletableEntity : BaseEntity
{
    [Column("is_deleted")] public bool IsDeleted { get; protected set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; protected set; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}