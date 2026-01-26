using Capychef.Api.Errors;
using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService, ILoggerFactory loggerFactory) : ControllerBase
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

        var logger = loggerFactory.CreateLogger("UserService.ValidateEmailAsync");

        if (error != null) return GlobalErrorHandler.handleError(error, logger);

        return Ok();
    }
}