using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserById(int id);
    Task<User?> GetTrackedUserById(int id);
    Task<bool> CheckUserExistsByUsernameAsync(string username);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetTrackedUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetTrackedUserByEmailAsync(string email);
    Task<bool> CheckUserExistsByEmailAsync(string email);
}