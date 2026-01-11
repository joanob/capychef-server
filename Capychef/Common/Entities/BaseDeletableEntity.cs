using System.ComponentModel.DataAnnotations.Schema;

namespace YourOwnBoss.Common.Entities;

public class BaseDeletableEntity : BaseEntity
{
    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}