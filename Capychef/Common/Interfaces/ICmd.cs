using Capychef.Common.Errors;

namespace Capychef.Common.Interfaces;

public interface ICmd
{
    ValidationError? Validate();
}