using Capychef.Common.Auth;
using Capychef.Infrastructure.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Cmd.Auth;
using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Errors;
using Capychef.Users.Domain.Interfaces;
using YourOwnBoss.Common.Entities;
using YourOwnBoss.Common.Errors;
using YourOwnBoss.Common.Result;

namespace Capychef.Users.Services;

public class AuthService(
    CapychefDbContext dbContext,
    IUserRepository userRepository,
    IUserPasswordRepository userPasswordRepository,
    IUserSessionRepository userSessionRepository,
    IUserTokenRepository userTokenRepository,
    IEmailSender emailSender) : IAuthService
{
    public async Task<Result<(UserDTO, AuthUserDetails)>> Signup(SignupCmd cmd)
    {
        var usernameInUse = await userRepository.CheckUserExistsByUsernameAsync(cmd.Username);
        if (usernameInUse) return new Result<(UserDTO, AuthUserDetails)>(new UsernameInUseError(cmd.Username));

        var user = new User(cmd);

        await userRepository.AddUserAsync(user);

        if (cmd.Password != null)
        {
            var password = new UserPassword(user, cmd.Password);

            await userPasswordRepository.AddUserPasswordAsync(password);
        }

        var session = new UserSession(user);

        await userSessionRepository.AddUserSessionAsync(session);

        if (cmd.Email != null)
        {
            var userToken = UserToken.CreateEmailValidationUserToken(user);

            await userTokenRepository.AddUserTokenAsync(userToken);

            await emailSender.SendEmailVerificationEmailAsync(cmd.Email, userToken.Token);
        }

        await dbContext.SaveChangesAsync();

        return new Result<(UserDTO, AuthUserDetails)>((new UserDTO(user), new AuthUserDetails(user.Id, session.Id)));
    }

    public async Task<Result<(UserDTO, AuthUserDetails)>> Login(LoginCmd cmd)
    {
        var user = await userRepository.FindUserByUsernameAsync(cmd.Username);
        if (user == null)
        {
            user = await userRepository.FindUserByEmailAsync(cmd.Username);
            if (user == null)
                return new Result<(UserDTO, AuthUserDetails)>(new NotFoundError(EntityType.User, cmd.Username));
        }

        var password = await userPasswordRepository.GetActiveUserPasswordByUserIdAsync(user.Id);
        if (password == null)
            return new Result<(UserDTO, AuthUserDetails)>(new NotFoundError(EntityType.User, user.Id));

        if (!password.checkPassword(cmd.Password))
            return new Result<(UserDTO, AuthUserDetails)>(new NotFoundError(EntityType.User,
                user.Id + " password mismatch"));

        var session = new UserSession(user);

        await userSessionRepository.AddUserSessionAsync(session);

        await dbContext.SaveChangesAsync();

        return new Result<(UserDTO, AuthUserDetails)>((new UserDTO(user), new AuthUserDetails(user.Id, session.Id)));
    }

    public async Task<AppError> RecoverPassword(string email)
    {
        var user = await userRepository.FindUserByEmailAsync(email);
        if (user == null) return new NotFoundError(EntityType.User, email);

        if (!user.IsEmailValid) return new NotFoundError(EntityType.User, user.Id + "email is not verified");

        var userToken = UserToken.CreatePasswordRecoveryUserToken(user);

        await userTokenRepository.AddUserTokenAsync(userToken);

        await emailSender.SendPasswordRecoveryEmailAsync(email, userToken.Token);

        await dbContext.SaveChangesAsync();

        return null;
    }
}