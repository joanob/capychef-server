using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Cmd.Auth;
using YourOwnBoss.Common.Entities;

namespace Capychef.Users.Domain.Entities;

[Table("users")]
public class User : BaseDeletableEntity
{
    public User()
    {
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

    [Column("username")] public string Username { get; set; }

    [Column("is_guest")] public bool IsGuest { get; set; }

    [Column("email")] public string? Email { get; set; }

    [Column("is_email_valid")] public bool IsEmailValid { get; set; }

    [Column("is_blocked")] public bool IsBlocked { get; set; }

    [Column("block_reason")] public string? BlockReason { get; set; }
}