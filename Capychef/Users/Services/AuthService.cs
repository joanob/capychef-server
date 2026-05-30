using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Infrastructure.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Errors;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Users.Services;

public class AuthService(
    CapychefDbContext dbContext,
    IUserRepository userRepository,
    IUserPasswordRepository userPasswordRepository,
    IUserSessionRepository userSessionRepository,
    IUserTokenRepository userTokenRepository,
    IHouseholdRepository householdRepository,
    IEmailSender emailSender) : IAuthService
{
    public async Task<Result<(UserDto, AuthUserDetails)>> Signup(SignupCmd cmd)
    {
        var error = cmd.Validate();

        if (error != null) return new Result<(UserDto, AuthUserDetails)>(error);

        var usernameInUse = await userRepository.CheckUserExistsByUsernameAsync(cmd.Username);

        if (usernameInUse) return new Result<(UserDto, AuthUserDetails)>(new UsernameInUseError(cmd.Username));

        if (!string.IsNullOrEmpty(cmd.Email))
        {
            var emailInUse = await userRepository.CheckUserExistsByEmailAsync(cmd.Email);

            if (emailInUse) return new Result<(UserDto, AuthUserDetails)>(new EmailInUseError(cmd.Email));
        }

        User user;

        if (cmd.Password != null)
        {
            if (cmd.Email != null)
                user = User.NewEmailUser(cmd.Username, cmd.Email);
            else
                user = User.NewPasswordUser(cmd.Username);

            var password = new UserPassword(user, cmd.Password);

            await userPasswordRepository.AddUserPasswordAsync(password);
        }
        else
        {
            user = User.NewGuestUser(cmd.Username);
        }

        await userRepository.AddUserAsync(user);

        var session = new UserSession(user);

        await userSessionRepository.AddUserSessionAsync(session);

        if (!string.IsNullOrEmpty(cmd.Email))
        {
            var userToken = UserToken.CreateEmailValidationUserToken(user);

            await userTokenRepository.AddUserTokenAsync(userToken);

            await emailSender.SendEmailVerificationEmailAsync(cmd.Email, userToken.Token);
        }

        await dbContext.SaveChangesAsync();

        return new Result<(UserDto, AuthUserDetails)>((new UserDto(user), new AuthUserDetails(user.Id, session.Id)));
    }

    public async Task<Result<(UserDto, AuthUserDetails)>> Login(LoginCmd cmd)
    {
        var error = cmd.Validate();

        if (error != null) return new Result<(UserDto, AuthUserDetails)>(error);

        var user = await userRepository.GetTrackedUserByUsernameOrEmailAsync(cmd.Username);
        if (user == null)
            return new Result<(UserDto, AuthUserDetails)>(new NotFoundError(EntityType.User, cmd.Username));

        var password = await userPasswordRepository.GetActiveUserPasswordByUserIdAsync(user.Id);
        if (password == null)
            return new Result<(UserDto, AuthUserDetails)>(new NotFoundError(EntityType.User, user.Id));

        if (!password.CheckPassword(cmd.Password))
            return new Result<(UserDto, AuthUserDetails)>(new NotFoundError(EntityType.User,
                user.Id + " password mismatch"));

        var session = new UserSession(user);

        await userSessionRepository.AddUserSessionAsync(session);

        await dbContext.SaveChangesAsync();

        return new Result<(UserDto, AuthUserDetails)>((new UserDto(user), new AuthUserDetails(user.Id, session.Id)));
    }

    public async Task<AppError?> RecoverPassword(string email)
    {
        var user = await userRepository.GetTrackedUserByEmailAsync(email);

        if (user == null) return new NotFoundError(EntityType.User, email);

        if (!user.IsEmailValid) return new NotFoundError(EntityType.User, user.Id + "email is not verified");

        var userToken = UserToken.CreatePasswordRecoveryUserToken(user.Id);

        await userTokenRepository.AddUserTokenAsync(userToken);

        try
        {
            await emailSender.SendPasswordRecoveryEmailAsync(email, userToken.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> ResetPassword(ResetPasswordCmd cmd)
    {
        var error = cmd.Validate();

        if (error != null) return error;

        var userToken = await userTokenRepository.GetTrackedUsableUserTokenByTokenAsync(cmd.PasswordRecoveryToken);

        if (userToken == null)
            return new NotFoundError(EntityType.Token, cmd.PasswordRecoveryToken);

        if (userToken.TokenType != UserTokenType.PasswordRecovery)
            return new NotFoundError(EntityType.Token, cmd.PasswordRecoveryToken);

        userToken.MarkUsed();

        var user = await userRepository.GetTrackedUserById(userToken.UserId);

        if (user == null) return new NotFoundError(EntityType.User, userToken.UserId);

        var oldPassword = await userPasswordRepository.GetTrackedActiveUserPasswordByUserIdAsync(user.Id);

        if (oldPassword == null) return new NotFoundError(EntityType.UserPassword, user.Id);

        oldPassword.IsActive = false;

        var password = new UserPassword(user, cmd.Password);

        await userPasswordRepository.AddUserPasswordAsync(password);

        var userSessions = await userSessionRepository.GetTrackedActiveUserSessionsByUserIdAsync(user.Id);

        foreach (var userSession in userSessions) userSession.IsRevoked = true;

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<Result<string>> GuestTransference(AuthUserDetails userDetails)
    {
        var user = await userRepository.GetTrackedUserById(userDetails.UserId);

        if (user == null) return new Result<string>(new NotFoundError(EntityType.User, userDetails.UserId));

        if (!user.IsGuest) return new Result<string>(new NotFoundError(EntityType.User, userDetails.UserId));

        var userToken = UserToken.CreateGuestAccountTransferUserToken(user.Id);

        await userTokenRepository.AddUserTokenAsync(userToken);

        await dbContext.SaveChangesAsync();

        return new Result<string>(userToken.Token);
    }

    public async Task<Result<(UserDto, AuthUserDetails)>> GuestLogin(GuestLoginCmd cmd)
    {
        var error = cmd.Validate();

        if (error != null) return new Result<(UserDto, AuthUserDetails)>(error);

        var userToken = await userTokenRepository.GetTrackedUsableUserTokenByTokenAsync(cmd.GuestTransferenceToken);

        if (userToken == null)
            return new Result<(UserDto, AuthUserDetails)>(new NotFoundError(EntityType.Token,
                cmd.GuestTransferenceToken));

        if (userToken.TokenType != UserTokenType.GuestAccountTransfer)
            return new Result<(UserDto, AuthUserDetails)>(new NotFoundError(EntityType.Token,
                cmd.GuestTransferenceToken));

        userToken.MarkUsed();

        var user = await userRepository.GetTrackedUserById(userToken.UserId);

        if (user == null)
            return new Result<(UserDto, AuthUserDetails)>(new NotFoundError(EntityType.User, userToken.UserId));

        var session = new UserSession(user);

        await userSessionRepository.AddUserSessionAsync(session);

        await dbContext.SaveChangesAsync();

        return new Result<(UserDto, AuthUserDetails)>((new UserDto(user), new AuthUserDetails(user.Id, session.Id)));
    }

    public async Task<Result<AuthSessionDto>> GetAuthSession(AuthUserDetails userDetails)
    {
        var user = await userRepository.GetUserById(userDetails.UserId);

        if (user == null)
            return new Result<AuthSessionDto>(new NotFoundError(EntityType.User, userDetails.UserId));

        Household? activeHousehold = null;

        if (userDetails.HasHouseholdId)
            activeHousehold = await householdRepository.GetHouseholdById(userDetails.GetHouseholdId());

        return new Result<AuthSessionDto>(new AuthSessionDto(user, activeHousehold));
    }
}