using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Users.Services;

public class UserService(
    CapychefDbContext dbContext,
    IUserRepository userRepository) : IUserService
{
    public async Task<bool> CheckUserByUsernameAsync(string username)
    {
        return await userRepository.CheckUserExistsByUsernameAsync(username);
    }
}