using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Utils;

namespace Capychef.Users.Domain.Entities;

[Table("users_tokens")]
public class UserToken
{
    [Column("id")] public int Id { get; init; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; init; }

    [Column("token")] [MaxLength(50)] public string Token { get; private set; } = "";

    [Column("token_type")] public UserTokenType TokenType { get; private set; }

    [Column("created_at")] public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("expires_at")] public DateTime? ExpiresAt { get; private set; }

    [Column("used_at")] public DateTime? UsedAt { get; set; }

    [Column("is_active")] public bool IsActive { get; set; }

    [Column("is_used")] public bool IsUsed { get; set; }

    public User User { get; private set; } = null!;

    public static UserToken CreatePasswordRecoveryUserToken(User user)
    {
        var userToken = new UserToken();

        userToken.User = user;
        userToken.Token = RandomGenerator.GenerateRandomAlphabetAndNumbersString(10);
        userToken.TokenType = UserTokenType.PasswordRecovery;
        userToken.CreatedAt = DateTime.Now;
        userToken.ExpiresAt = user.CreatedAt.AddHours(24);
        userToken.UsedAt = null;
        userToken.IsActive = true;
        userToken.IsUsed = false;

        return userToken;
    }

    public static UserToken CreateEmailValidationUserToken(User user)
    {
        var userToken = new UserToken();

        userToken.User = user;
        userToken.Token = RandomGenerator.GenerateRandomCapsAndNumbersString(6);
        userToken.TokenType = UserTokenType.EmailValidation;
        userToken.CreatedAt = DateTime.Now;
        userToken.ExpiresAt = user.CreatedAt.AddDays(30);
        userToken.UsedAt = null;
        userToken.IsActive = true;
        userToken.IsUsed = false;

        return userToken;
    }

    public static UserToken CreateGuestAccountTransferUserToken(User user)
    {
        var userToken = new UserToken();

        userToken.User = user;
        userToken.Token = RandomGenerator.GenerateRandomCapsString(8);
        userToken.TokenType = UserTokenType.GuestAccountTransfer;
        userToken.CreatedAt = DateTime.Now;
        userToken.ExpiresAt = user.CreatedAt.AddHours(24);
        userToken.UsedAt = null;
        userToken.IsActive = true;
        userToken.IsUsed = false;

        return userToken;
    }

    public void MarkUsed()
    {
        IsUsed = true;
        UsedAt = DateTime.Now;
        IsActive = false;
    }
}

public enum UserTokenType
{
    PasswordRecovery = 1,
    EmailValidation = 2,
    GuestAccountTransfer = 3
}

public static class UserTokenExtensions
{
    public static IQueryable<UserToken> Usable(this IQueryable<UserToken> userTokens)
    {
        return userTokens.Where(x => x.IsActive && (x.ExpiresAt == null || x.ExpiresAt > DateTime.UtcNow));
    }
}