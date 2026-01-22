using Capychef.Common.Auth;

namespace Capychef.Api.Auth;

public class AuthUserDetailsService
{
    private const string contextItemName = "AuthUserDetailsService";

    public static void AddAuthUserDetailsToContext(HttpContext context, AuthUserDetails userDetails)
    {
        context.Items[contextItemName] = userDetails;
    }

    public static AuthUserDetails GetAuthUserDetailsFromContext(HttpContext context)
    {
        return context.Items[contextItemName] as AuthUserDetails;
    }
}