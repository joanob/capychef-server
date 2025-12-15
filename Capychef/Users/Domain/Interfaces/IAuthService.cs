using Capychef.Common.Auth;
using Capychef.Users.Domain.Cmd.Auth;
using Capychef.Users.Domain.DTO;
using YourOwnBoss.Common.Errors;
using YourOwnBoss.Common.Result;

namespace Capychef.Users.Domain.Interfaces;

public interface IAuthService
{
    Task<Result<(UserDTO, AuthUserDetails)>> Signup(SignupCmd cmd);
    Task<Result<(UserDTO, AuthUserDetails)>> Login(LoginCmd cmd);
    Task<AppError> RecoverPassword(string email);
    Task<AppError> ResetPassword(ResetPasswordCmd cmd);
    Task<Result<string>> GuestTransference(AuthUserDetails userDetails);
    Task<Result<(UserDTO, AuthUserDetails)>> GuestLogin(GuestLoginCmd cmd);
}