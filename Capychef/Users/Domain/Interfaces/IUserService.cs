using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.DTO;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserService
{
    Task<bool> CheckUserByUsernameAsync(string username);
    Task<AppError?> ValidateEmailAsync(string token);
    Task<Result<(UserDto, AuthUserDetails)>> ChangePassword(AuthUserDetails userDetails, ChangePasswordCmd cmd);
    Task<AppError?> DeleteAccount(AuthUserDetails userDetails, DeleteAccountCmd cmd);
}