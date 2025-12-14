using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Capychef.Users.Domain.Entities;

[Table("users_tokens")]
public class UserToken
{
    private static readonly string capsAndNumbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private static readonly string
        alphabetAndNumbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    [Column("id")] public int Id { get; private set; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; private set; }

    [Column("token")] public string Token { get; private set; }

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
        userToken.createRandomToken(10, alphabetAndNumbers);
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
        userToken.createRandomToken(6, capsAndNumbers);
        userToken.TokenType = UserTokenType.PasswordRecovery;
        userToken.CreatedAt = DateTime.Now;
        userToken.ExpiresAt = user.CreatedAt.AddHours(24);
        userToken.UsedAt = null;
        userToken.IsActive = true;
        userToken.IsUsed = false;

        return userToken;
    }

    public static UserToken CreateGuestAccountTransferUserToken(User user)
    {
        var userToken = new UserToken();

        userToken.User = user;
        userToken.createRandomToken(8, capsAndNumbers);
        userToken.TokenType = UserTokenType.PasswordRecovery;
        userToken.CreatedAt = DateTime.Now;
        userToken.ExpiresAt = user.CreatedAt.AddHours(24);
        userToken.UsedAt = null;
        userToken.IsActive = true;
        userToken.IsUsed = false;

        return userToken;
    }

    private void createRandomToken(int length, string characterSet)
    {
        var resut = new char[length];
        var buffer = new byte[length];

        RandomNumberGenerator.Fill(buffer);

        for (var i = 0; i < length; i++) resut[i] = characterSet[buffer[i] % characterSet.Length];

        Token = new string(resut);
    }
}

public enum UserTokenType
{
    PasswordRecovery = 1,
    EmailValidation = 2,
    GuestAccountTransfer = 3
}