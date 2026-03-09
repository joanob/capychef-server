using Capychef.Common.Errors;

namespace Capychef.Gourmet.Domain.Errors;

public class SubscriptionRequiredError : AppError
{
    public SubscriptionRequiredError(string message) : base(ErrorType.SubscriptionRequired)
    {
        Message = message;
    }
}