using Capychef.Common.Auth;

namespace Capychef.Api.Auth;

public class AuthUserDetailsService
{
    private const string ContextItemName = "AuthUserDetailsService";

    public static void AddAuthUserDetailsToContext(HttpContext context, AuthUserDetails userDetails)
    {
        context.Items[ContextItemName] = userDetails;
    }

    public static AuthUserDetails GetAuthUserDetailsFromContext(HttpContext context)
    {
        var userDetails = context.Items[ContextItemName];

        return userDetails as AuthUserDetails ?? new AuthUserDetails();
    }
}