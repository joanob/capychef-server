namespace YourOwnBoss.Common.Entities;

public class BaseEntity
{
    public int Id { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public int RowVersion { get; private set; }
}