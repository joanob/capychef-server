using Capychef.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Errors;

public static class GlobalErrorHandler
{
    public static ActionResult HandleError(AppError error, ILogger logger)
    {
        logger.LogError(error.ToString());

        int statusCode;

        if (error.ErrorType.Equals(ErrorType.CannotCreate) || error.ErrorType.Equals(ErrorType.Validation))
            statusCode = 400;
        else if (error.ErrorType.Equals(ErrorType.Authentication))
            statusCode = 401;
        else if (error.ErrorType.Equals(ErrorType.Authorization))
            statusCode = 403;
        else if (error.ErrorType.Equals(ErrorType.SubscriptionRequired))
            statusCode = 403;
        else if (error.ErrorType.Equals(ErrorType.NotFound))
            statusCode = 404;
        else
            statusCode = 500;

        return new ObjectResult(new ApiResponse<int>(new ApiError(error))) { StatusCode = statusCode };
    }
}