using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Users.Domain.Cmd;

namespace Capychef.Users.Domain.Entities;

[Table("users")]
public class User : BaseDeletableEntity
{
    protected User()
    {
        Username = null!;
    }

    public User(SignupCmd signupCmd)
    {
        Username = signupCmd.Username;
        IsGuest = signupCmd.Password == null;
        Email = signupCmd.Email;
        IsEmailValid = false;
        IsBlocked = false;
        BlockReason = null;
    }

    [Column("username")] [MaxLength(50)] public string Username { get; set; }

    [Column("is_guest")] public bool IsGuest { get; set; }

    [Column("email")] [MaxLength(255)] public string? Email { get; set; }

    [Column("is_email_valid")] public bool IsEmailValid { get; set; }

    [Column("is_blocked")] public bool IsBlocked { get; set; }

    [Column("block_reason")]
    [MaxLength(5000)]
    public string? BlockReason { get; set; }
}

public static class UserExtensions
{
    public static IQueryable<User> Active(this IQueryable<User> users)
    {
        return users.Where(x => !x.IsDeleted && !x.IsBlocked);
    }
}