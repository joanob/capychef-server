using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Users.Domain.Cmd;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserService
{
    Task<bool> CheckUserByUsernameAsync(string username);
    Task<AppError?> ValidateEmailAsync(string token);
    Task<AppError?> DeleteAccount(AuthUserDetails userDetails, DeleteAccountCmd cmd);
}