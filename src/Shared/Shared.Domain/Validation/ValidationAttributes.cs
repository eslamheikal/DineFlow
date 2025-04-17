using System.ComponentModel.DataAnnotations;

namespace Shared.Domain.Validation;

public class RequiredGuidAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null) return false;
        return Guid.TryParse(value.ToString(), out _);
    }
}

public class PhoneNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null) return true;
        var phoneNumber = value.ToString();
        return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$");
    }
}

public class PasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null) return false;
        var password = value.ToString();
        return password.Length >= 8 &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[^a-zA-Z0-9]");
    }
} 