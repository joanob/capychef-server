using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Users.Repositories;

public class UserSessionRepository(CapychefDbContext dbContext) : IUserSessionRepository
{
    public async Task AddUserSessionAsync(UserSession session)
    {
        await dbContext.UsersSessions.AddAsync(session);
    }

    public async Task<UserSession?> GetTrackedUserSessionByIdAsync(int id)
    {
        return await dbContext.UsersSessions.FirstOrDefaultAsync(session => session.Id == id);
    }

    public async Task<List<UserSession>> GetTrackedActiveUserSessionsByUserIdAsync(int userId)
    {
        return await dbContext.UsersSessions.Where(session => session.UserId == userId && !session.IsRevoked)
            .ToListAsync();
    }
}