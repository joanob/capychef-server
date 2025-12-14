using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<bool> CheckUserExistsByUsernameAsync(string username);
    Task<User?> FindUserByUsernameAsync(string username);
    Task<User?> FindUserByEmailAsync(string username);
}