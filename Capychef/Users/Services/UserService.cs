using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Persistence;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Errors;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Users.Services;

public class UserService(
    CapychefDbContext dbContext,
    IUserRepository userRepository,
    IUserTokenRepository userTokenRepository,
    IUserPasswordRepository userPasswordRepository,
    IUserSessionRepository userSessionRepository) : IUserService
{
    public async Task<bool> CheckUserByUsernameAsync(string username)
    {
        username = username.Trim();

        if (string.IsNullOrEmpty(username) || username.Length > 50)
            return false;

        return await userRepository.CheckUserExistsByUsernameAsync(username);
    }

    public async Task<AppError?> ValidateEmailAsync(string token)
    {
        var userToken = await userTokenRepository.GetTrackedUsableUserTokenByTokenAsync(token);

        if (userToken == null)
            return new NotFoundError(EntityType.Token, token);

        if (userToken.TokenType != UserTokenType.EmailValidation) return new NotFoundError(EntityType.Token, token);

        userToken.MarkUsed();

        var user = await userRepository.GetTrackedUserById(userToken.UserId);

        if (user == null) return new NotFoundError(EntityType.User, userToken.UserId);

        user.IsEmailValid = true;

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> DeleteAccount(AuthUserDetails userDetails, DeleteAccountCmd cmd)
    {
        var user = await userRepository.GetTrackedUserById(userDetails.UserId);
        if (user == null) return new NotFoundError(EntityType.User, userDetails.UserId);

        if (!user.IsGuest)
        {
            if (string.IsNullOrEmpty(cmd.Password))
                return new ValidationError("Password is required to delete a non-guest account");

            var password = await userPasswordRepository.GetActiveUserPasswordByUserIdAsync(user.Id);
            if (password == null) return new NotFoundError(EntityType.UserPassword, user.Id);

            if (!password.CheckPassword(cmd.Password))
                return new IncorrectPasswordError(user.Id);
        }

        var sessions = await userSessionRepository.GetTrackedActiveUserSessionsByUserIdAsync(user.Id);
        foreach (var session in sessions) session.IsRevoked = true;

        user.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }
}