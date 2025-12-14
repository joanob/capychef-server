using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("username/check/{username}")]
    public async Task<ActionResult<UserDTO>> Signup(string username)
    {
        var result = await userService.CheckUserByUsernameAsync(username);

        return result ? Ok() : NotFound();
    }

    [HttpGet("email/validate/{token}")]
    public async Task<ActionResult> ValidateEmail(string token)
    {
        var error = await userService.ValidateEmailAsync(token);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }
}