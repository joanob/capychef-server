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

    public async Task<User?> FindUserByUsernameAsync(string username)
    {
        return await dbContext.Users.Active().Where(x => x.Username == username).FirstOrDefaultAsync();
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await dbContext.Users.Active().Where(x => x.Email == email).FirstOrDefaultAsync();
    }
}