namespace Capychef.Api.Errors;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            Console.WriteLine(e);
        }
    }
}