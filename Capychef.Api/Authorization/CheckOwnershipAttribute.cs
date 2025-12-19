using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Authorization;

public class CheckOwnershipAttribute : TypeFilterAttribute
{
    public CheckOwnershipAttribute()
        : base(typeof(CheckOwnershipFilter))
    {
    }
}