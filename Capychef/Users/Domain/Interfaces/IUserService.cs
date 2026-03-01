using Capychef.Common.Auth;
using Capychef.Common.Errors;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserService
{
    Task<bool> CheckUserByUsernameAsync(string username);
    Task<AppError?> ValidateEmailAsync(string token);
    Task<AppError> SetWelcomeComplete(AuthUserDetails userDetails);
}