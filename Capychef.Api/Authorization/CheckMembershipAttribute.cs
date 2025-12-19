using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Authorization;

public class CheckMembershipAttribute : TypeFilterAttribute
{
    public CheckMembershipAttribute()
        : base(typeof(CheckOwnershipFilter))
    {
    }
}