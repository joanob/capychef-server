using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.DTO;

public class UserDTO
{
    public UserDTO(User user)
    {
        Id = user.Id;
        Username = user.Username;
    }

    public int Id { get; }
    public string Username { get; }
}