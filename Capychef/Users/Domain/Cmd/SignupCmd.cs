using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Capychef.Common.Errors;
using Capychef.Common.Interfaces;

namespace Capychef.Users.Domain.Cmd;

public class SignupCmd: ICmd
{
    public string Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
    
    public ValidationError? Validate()
    {
        Username = Username?.Trim();
        Email = Email?.Trim();
        Password = Password?.Trim();
        
        if (string.IsNullOrEmpty(Username))
        {
            return new ValidationError("SignupCmd username is null or empty");
        }

        if (!string.IsNullOrEmpty(Email))
        {
            var emailRegex = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            if (!emailRegex.IsMatch(Email))
            {
                return new ValidationError("SignupCmd email " + Email + " is invalid");
            }

            if (string.IsNullOrEmpty(Password))
            {
                return new ValidationError("SignupCmd password is null or empty when email is provided");
            }
        }

        return null;
    }
}