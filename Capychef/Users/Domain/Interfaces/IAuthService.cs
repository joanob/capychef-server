using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.DTO;

namespace Capychef.Users.Domain.Interfaces;

public interface IAuthService
{
    Task<Result<(UserDTO, AuthUserDetails)>> Signup(SignupCmd cmd);
    Task<Result<(UserDTO, AuthUserDetails)>> Login(LoginCmd cmd);
    Task<AppError> RecoverPassword(string email);
    Task<AppError> ResetPassword(ResetPasswordCmd cmd);
    Task<Result<string>> GuestTransference(AuthUserDetails userDetails);
    Task<Result<(UserDTO, AuthUserDetails)>> GuestLogin(GuestLoginCmd cmd);
    Task<AuthSessionDTO> GetAuthSession(AuthUserDetails userDetails);
}