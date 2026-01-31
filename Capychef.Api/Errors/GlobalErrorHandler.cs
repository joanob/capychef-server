using Capychef.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Errors;

public class GlobalErrorHandler
{
    public static ActionResult handleError(AppError error, ILogger logger)
    {
        logger.LogError(error.ToString());

        var statusCode = 0;

        if (error.ErrorType.Equals(ErrorType.CannotCreate))
            statusCode = 400;
        else if (error.ErrorType.Equals(ErrorType.Authentication))
            statusCode = 401;
        else if (error.ErrorType.Equals(ErrorType.Authorization))
            statusCode = 403;
        else if (error.ErrorType.Equals(ErrorType.NotFound))
            statusCode = 404;
        else
            statusCode = 500;

        return new ObjectResult(error) { StatusCode = statusCode };
    }
}