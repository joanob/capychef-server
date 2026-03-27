using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Errors;
using JetBrains.Annotations;

namespace Capychef.Users.Domain.Entities;

[Table("users_passwords")]
public class UserPassword
{
    public UserPassword(User user, string password)
    {
        User = user;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    }

    [Column("id")] public int Id { get; init; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; init; }

    [UsedImplicitly]
    [Column("password")]
    [MaxLength(500)]
    public string Password { get; private set; }

    [Column("created_at")] public DateTime CreatedAt { get; private set; }

    [Column("is_active")] public bool IsActive { get; set; }

    public User User { get; private set; }

    public static ValidationError? ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return new ValidationError("Password is empty.");

        if (password.Length > 1024)
            return new ValidationError("Password is too long. Max length is 1024 characters.");

        return null;
    }

    public bool CheckPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password);
    }
}