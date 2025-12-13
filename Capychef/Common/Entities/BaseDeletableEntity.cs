namespace YourOwnBoss.Common.Entities;

public class BaseDeletableEntity : BaseEntity
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}