using Capychef.Users.Domain.Cmd.Auth;
using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("check/username/{username}")]
    public async Task<ActionResult<UserDTO>> Signup(string username)
    {
        var result = await userService.CheckUserByUsernameAsync(username);

        return result ? Ok(): NotFound();
    }
}