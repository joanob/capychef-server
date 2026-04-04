using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Utils;

namespace Capychef.Users.Domain.Entities;

[Table("users_tokens")]
public class UserToken
{
    protected UserToken()
    {
        Token = "";
    }

    private UserToken(int userId, string token, UserTokenType tokenType, DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        TokenType = tokenType;
        ExpiresAt = expiresAt;
        IsActive = true;
        IsUsed = false;
    }

    private UserToken(User user, string token, UserTokenType tokenType, DateTime expiresAt)
    {
        User = user;
        Token = token;
        TokenType = tokenType;
        ExpiresAt = expiresAt;
        IsActive = true;
        IsUsed = false;
    }

    [Column("id")] public int Id { get; init; }

    [Column("user_id")] public int UserId { get; init; }

    [Column("token")] [MaxLength(20)] public string Token { get; init; }

    [Column("token_type")] public UserTokenType TokenType { get; init; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("expires_at")] public DateTime? ExpiresAt { get; init; }

    [Column("is_active")] public bool IsActive { get; private set; }

    [Column("is_used")] public bool IsUsed { get; set; }

    [Column("used_at")] public DateTime? UsedAt { get; private set; }

    [ForeignKey(nameof(UserId))] public User? User { get; init; }

    public static UserToken CreatePasswordRecoveryUserToken(int userId)
    {
        var token = RandomGenerator.GenerateRandomAlphabetAndNumbersString(10);

        var userToken = new UserToken(userId, token, UserTokenType.PasswordRecovery, DateTime.Now.AddDays(1));

        return userToken;
    }

    public static UserToken CreateEmailValidationUserToken(User user)
    {
        var token = RandomGenerator.GenerateRandomAlphabetAndNumbersString(6);

        var userToken = new UserToken(user, token, UserTokenType.EmailValidation, DateTime.Now.AddDays(30));

        return userToken;
    }

    public static UserToken CreateGuestAccountTransferUserToken(int userId)
    {
        var token = RandomGenerator.GenerateRandomAlphabetAndNumbersString(8);

        var userToken = new UserToken(userId, token, UserTokenType.GuestAccountTransfer, DateTime.Now.AddDays(1));

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