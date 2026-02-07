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

    public static List<UserDTO> ToDTOList(List<User> users)
    {
        return users.Select(u => new UserDTO(u)).ToList();
    }
}