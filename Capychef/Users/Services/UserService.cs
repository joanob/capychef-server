using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Interfaces;
using YourOwnBoss.Common.Entities;
using YourOwnBoss.Common.Errors;

namespace Capychef.Users.Services;

public class UserService(
    CapychefDbContext dbContext,
    IUserRepository userRepository,
    IUserTokenRepository userTokenRepository) : IUserService
{
    public async Task<bool> CheckUserByUsernameAsync(string username)
    {
        return await userRepository.CheckUserExistsByUsernameAsync(username);
    }

    public async Task<AppError?> ValidateEmailAsync(string token)
    {
        var userToken = await userTokenRepository.GetTrackedUsableUserTokenByTokenAsync(token);

        if (userToken == null)
            return new NotFoundError(EntityType.Token, token);

        if (userToken.TokenType != UserTokenType.EmailValidation) return new NotFoundError(EntityType.Token, token);

        userToken.markUsed();

        var user = await userRepository.GetTrackedUserById(userToken.UserId);

        if (user == null) return new NotFoundError(EntityType.User, userToken.UserId);

        user.IsEmailValid = true;

        await dbContext.SaveChangesAsync();

        return null;
    }
}