using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Users.Repositories;

public class UserRepository(CapychefDbContext dbContext) : IUserRepository
{
    public async Task AddUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
    }

    public async Task<bool> CheckUserExistsByUsernameAsync(string username)
    {
        return await dbContext.Users.Where(x => x.Username == username).AnyAsync();
    }
}