using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.DTO;

public class UserDto
{
    public UserDto(User user)
    {
        Id = user.Id;
        Username = user.Username;
        IsGuest = user.IsGuest;
        Email = user.Email;
        IsEmailValid = user.IsEmailValid;
        CreatedAt = user.CreatedAt;
    }

    public int Id { get; }
    public string Username { get; }
    public bool IsGuest { get; }
    public string? Email { get; }
    public bool IsEmailValid { get; }
    public DateTime CreatedAt { get; }

    public static List<UserDto> ToDtoList(List<User> users)
    {
        return users.Select(u => new UserDto(u)).ToList();
    }
}