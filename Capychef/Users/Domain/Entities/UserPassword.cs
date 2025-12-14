using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Users.Domain.Entities;

[Table("users_passwords")]
public class UserPassword
{
    public UserPassword()
    {
    }

    public UserPassword(User user, string password)
    {
        User = user;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    }

    [Column("id")] public int Id { get; private set; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; private set; }

    [Column("password")] public string Password { get; }

    [Column("created_at")] public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("is_active")] public bool IsActive { get; set; }

    public User User { get; private set; } = null!;

    public bool checkPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password);
    }
}