using Capychef.Common.Auth;
using Capychef.Common.Errors;

namespace Capychef.Gourmet.Domain.Interfaces;

public interface ISubscriptionService
{
    Task<AppError?> CheckUserCanCreateHousehold(AuthUserDetails userDetails);
}