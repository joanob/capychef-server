using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Users.Repositories;

public class UserPasswordRepository(CapychefDbContext dbContext) : IUserPasswordRepository
{
    public async Task AddUserPasswordAsync(UserPassword password)
    {
        await dbContext.UsersPasswords.AddAsync(password);
    }

    public async Task<UserPassword?> GetActiveUserPasswordByUserIdAsync(int userId)
    {
        return await dbContext.UsersPasswords.Where(x => x.UserId == userId && x.IsActive).FirstOrDefaultAsync();
    }
}