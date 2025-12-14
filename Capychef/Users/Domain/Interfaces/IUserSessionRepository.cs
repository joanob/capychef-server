using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserSessionRepository
{
    Task AddUserSessionAsync(UserSession session);
    Task<UserSession> GetTrackedUserSessionByIdAsync(int id);
}