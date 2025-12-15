using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserPasswordRepository
{
    Task AddUserPasswordAsync(UserPassword password);
    Task<UserPassword?> GetActiveUserPasswordByUserIdAsync(int userId);
    Task<UserPassword?> GetTrackedActiveUserPasswordByUserIdAsync(int userId);
}