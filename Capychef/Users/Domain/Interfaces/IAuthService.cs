using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.DTO;

namespace Capychef.Users.Domain.Interfaces;

public interface IAuthService
{
    Task<Result<(UserDto, AuthUserDetails)>> Signup(SignupCmd cmd);
    Task<Result<(UserDto, AuthUserDetails)>> Login(LoginCmd cmd);
    Task<AppError?> RecoverPassword(string email);
    Task<AppError?> ResetPassword(ResetPasswordCmd cmd);
    Task<Result<(UserDto, AuthUserDetails)>> ChangePassword(AuthUserDetails userDetails, ChangePasswordCmd cmd);
    Task<Result<string>> GuestTransference(AuthUserDetails userDetails);
    Task<Result<(UserDto, AuthUserDetails)>> GuestLogin(GuestLoginCmd cmd);
    Task<Result<AuthSessionDto>> GetAuthSession(AuthUserDetails userDetails);
}