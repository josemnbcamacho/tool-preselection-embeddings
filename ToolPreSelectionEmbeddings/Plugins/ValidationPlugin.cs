using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class ValidationPlugin
{
    [KernelFunction, Description("Validates email addresses")]
    public bool ValidateEmail([Description("The email address to validate")] string email)
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    [KernelFunction, Description("Validates phone numbers")]
    public bool ValidatePhone([Description("The phone number to validate")] string phone)
    {
        var pattern = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";
        return Regex.IsMatch(phone, pattern);
    }

    [KernelFunction, Description("Validates URLs")]
    public bool ValidateUrl([Description("The URL to validate")] string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    [KernelFunction, Description("Validates a credit card number using Luhn algorithm")]
    public bool ValidateCreditCard([Description("The credit card number to validate")] string cardNumber)
    {
        var number = cardNumber.Replace(" ", "").Replace("-", "");
        if (!number.All(char.IsDigit)) return false;
        
        int sum = 0;
        bool alternate = false;
        for (int i = number.Length - 1; i >= 0; i--)
        {
            int n = int.Parse(number[i].ToString());
            if (alternate)
            {
                n *= 2;
                if (n > 9) n -= 9;
            }
            sum += n;
            alternate = !alternate;
        }
        return sum % 10 == 0;
    }

    [KernelFunction, Description("Validates an ISBN number")]
    public bool ValidateISBN([Description("The ISBN number to validate")] string isbn)
    {
        var number = isbn.Replace("-", "").Replace(" ", "");
        if (number.Length != 10 && number.Length != 13) return false;
        
        if (number.Length == 10)
        {
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (10 - i) * (number[i] - '0');
            
            var lastChar = number[9];
            sum += (lastChar == 'X' || lastChar == 'x') ? 10 : (lastChar - '0');
            return sum % 11 == 0;
        }
        else
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += (i % 2 == 0 ? 1 : 3) * (number[i] - '0');
            
            var checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (number[12] - '0');
        }
    }

    [KernelFunction, Description("Validates a password strength")]
    public (bool IsValid, string Reason) ValidatePasswordStrength(
        [Description("The password to validate")] string password)
    {
        if (password.Length < 8)
            return (false, "Password must be at least 8 characters long");
        
        if (!password.Any(char.IsUpper))
            return (false, "Password must contain at least one uppercase letter");
        
        if (!password.Any(char.IsLower))
            return (false, "Password must contain at least one lowercase letter");
        
        if (!password.Any(char.IsDigit))
            return (false, "Password must contain at least one number");
        
        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            return (false, "Password must contain at least one special character");
        
        return (true, "Password meets all requirements");
    }

    [KernelFunction, Description("Validates a date string format")]
    public bool ValidateDate(
        [Description("The date string to validate")] string date,
        [Description("The expected format (e.g., yyyy-MM-dd)")] string format)
    {
        return DateTime.TryParseExact(date, format, null, 
            System.Globalization.DateTimeStyles.None, out _);
    }
} 