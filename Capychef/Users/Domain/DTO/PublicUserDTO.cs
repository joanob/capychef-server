using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.DTO;

public class PublicUserDto
{
    public PublicUserDto(User user)
    {
        Id = user.Id;
        Username = user.Username;
    }

    public int Id { get; }
    public string Username { get; }

    public static List<PublicUserDto> ToDtoList(List<User> users)
    {
        return users.Select(u => new PublicUserDto(u)).ToList();
    }
}