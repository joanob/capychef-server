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
        LastRefreshAt = DateTime.UtcNow;
        IsRevoked = false;
    }

    [Column("id")] public int Id { get; init; }

    [Column("user_id")] public int UserId { get; init; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("last_refresh_at")] public DateTime LastRefreshAt { get; set; }

    [Column("is_revoked")] public bool IsRevoked { get; set; }

    [ForeignKey(nameof(UserId))] public User? User { get; init; }
}