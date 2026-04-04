using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Users.Domain.Entities;

[Table("users")]
public class User
{
    protected User()
    {
        Username = "";
    }

    [Column("id")] public int Id { get; init; }

    [Column("username")] [MaxLength(50)] public string Username { get; set; }

    [Column("is_guest")] public bool IsGuest { get; set; }

    [Column("email")] [MaxLength(255)] public string? Email { get; set; }

    [Column("is_email_valid")] public bool IsEmailValid { get; set; }

    [Column("is_blocked")] public bool IsBlocked { get; set; }

    [Column("block_reason")]
    [MaxLength(5000)]
    public string? BlockReason { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("row_version")] public int RowVersion { get; init; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    public static User NewPasswordUser(string username)
    {
        return new User
        {
            Username = username,
            IsGuest = false,
            Email = null,
            IsEmailValid = false,
            IsBlocked = false,
            BlockReason = null
        };
    }

    public static User NewEmailUser(string username, string email)
    {
        return new User
        {
            Username = username,
            IsGuest = false,
            Email = email,
            IsEmailValid = false,
            IsBlocked = false,
            BlockReason = null
        };
    }

    public static User NewGuestUser(string username)
    {
        return new User
        {
            Username = username,
            IsGuest = true,
            Email = null,
            IsEmailValid = false,
            IsBlocked = false,
            BlockReason = null
        };
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}

public static class UserExtensions
{
    public static IQueryable<User> Active(this IQueryable<User> users)
    {
        return users.Where(x => !x.IsDeleted && !x.IsBlocked);
    }
}