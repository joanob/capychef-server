using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Users.Domain.Entities;

[Table("users_sessions")]
public class UserSession
{
    protected UserSession()
    {
    }

    public UserSession(User user)
    {
        User = user;
        CreatedAt = DateTime.UtcNow;
        LastConnectionAt = DateTime.UtcNow;
        IsRevoked = false;
    }

    [Column("id")] public int Id { get; init; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; init; }

    [Column("created_at")] public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("last_connection_at")] public DateTime LastConnectionAt { get; set; }

    [Column("is_revoked")] public bool IsRevoked { get; set; }

    public User User { get; private set; } = null!;
}