using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Errors;
using JetBrains.Annotations;

namespace Capychef.Users.Domain.Entities;

[Table("users_passwords")]
public class UserPassword
{
    protected UserPassword()
    {
    }

    public UserPassword(User user, string password)
    {
        User = user;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    }

    [Column("id")] public int Id { get; init; }

    [Column("user_id")]
    public int UserId { get; init; }

    [Column("password")]
    [MaxLength(255)]
    protected string Password { get; init; }

    [Column("created_at")] public DateTime CreatedAt { get; init; }

    [Column("is_active")] public bool IsActive { get; set; }

    [ForeignKey(nameof(UserId))] public User? User { get; init; }

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