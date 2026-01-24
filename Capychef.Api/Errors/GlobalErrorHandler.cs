using Capychef.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Errors;

public class GlobalErrorHandler
{
    public static ActionResult handleError(AppError error, ILogger logger)
    {
        logger.LogError(error.ToString());

        if (error is NotFoundError) return new StatusCodeResult(404);

        switch (error.errorType)
        {
            case ErrorType.CANNOT_CREATE: return new StatusCodeResult(400);
            case ErrorType.AUTHENTICATION: return new StatusCodeResult(401);
            case ErrorType.AUTHORIZATION: return new StatusCodeResult(403);
            case ErrorType.NOT_FOUND: return new StatusCodeResult(404);
            case ErrorType.OTHER_ERROR: return new StatusCodeResult(500);
            default: return new StatusCodeResult(400);
        }
    }
}