using YourOwnBoss.Game.Users.Domain.Interfaces;

namespace YourOwnBoss.Common.Auth;

public class AuthMiddleware(RequestDelegate next)
{
    private readonly string[] publicRoutes =
    {
        "/auth/signup",
        "/users/username/check",
        "/auth/login"
    };

    public async Task InvokeAsync(HttpContext context)
    {
        if (publicRoutes.Any(x => x.StartsWith(context.Request.Path.Value)))
        {
            await next(context);
            return;
        }

        var userDetails = JWTService.getUserDetailsFromSessionJWT(context.Request);
        if (userDetails != null)
        {
            AuthUserDetailsService.AddAuthUserDetailsToContext(context, userDetails);
            await next(context);
            return;
        }

        userDetails = JWTService.getUserDetailsFromRefreshJWT(context.Request);
        if (userDetails != null)
        {
            var userSesionService = context.RequestServices.GetRequiredService<IUserSessionService>();
            if (await userSesionService.ValidateAuthUserSession(userDetails))
            {
                JWTService.CreateAndSendJWT(userDetails, context.Response);
                AuthUserDetailsService.AddAuthUserDetailsToContext(context, userDetails);
                await next(context);
                return;
            }
        }

        context.Response.StatusCode = 401;
    }
}