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

    public async Task<bool> CheckUserExistsByEmailAsync(string email)
    {
        return await dbContext.Users.Where(x => x.Email == email).AnyAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await dbContext.Users.AsNoTracking().Active().Where(x => x.Username == username).FirstOrDefaultAsync();
    }

    public async Task<User> GetTrackedUserByUsernameAsync(string username)
    {
        return await dbContext.Users.Active().Where(x => x.Username == username).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await dbContext.Users.AsNoTracking().Active().Where(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> GetTrackedUserByEmailAsync(string email)
    {
        return await dbContext.Users.Active().Where(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserById(int id)
    {
        return await dbContext.Users.AsNoTracking().Active().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetTrackedUserById(int id)
    {
        return await dbContext.Users.Active().Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}