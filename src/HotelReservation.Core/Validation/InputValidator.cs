using System.Globalization;
using System.Text.RegularExpressions;

namespace HotelReservation.Core.Validation;

public static partial class InputValidator
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")]
    private static partial Regex EmailPattern();

    [GeneratedRegex(@"^\+?[0-9][0-9 /-]{5,19}$")]
    private static partial Regex PhonePattern();

    public static bool IsValidName(string? value) =>
        !string.IsNullOrWhiteSpace(value) && value.Trim().Length <= 50;

    public static bool IsValidEmail(string? value) =>
        !string.IsNullOrWhiteSpace(value) && value.Length <= 150 && EmailPattern().IsMatch(value.Trim());

    public static bool IsValidPhone(string? value) =>
        !string.IsNullOrWhiteSpace(value) && PhonePattern().IsMatch(value.Trim());

    public static bool IsStrongPassword(string? value) =>
        value is { Length: >= 8 } && value.Any(char.IsLetter) && value.Any(char.IsDigit);
}

public static partial class CardValidator
{
    [GeneratedRegex(@"^[0-9 -]+$")]
    private static partial Regex CardNumberPattern();

    [GeneratedRegex(@"^(0[1-9]|1[0-2])\s*/\s*([0-9]{2})$")]
    private static partial Regex ExpiryPattern();

    [GeneratedRegex(@"^[0-9]{3,4}$")]
    private static partial Regex CvvPattern();

    public static string DigitsOnly(string value) =>
        new(value.Where(char.IsDigit).ToArray());

    public static bool IsValidNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || !CardNumberPattern().IsMatch(value))
        {
            return false;
        }

        var digits = DigitsOnly(value);
        if (digits.Length is < 13 or > 19)
        {
            return false;
        }

        var sum = 0;
        var doubleDigit = false;

        for (var index = digits.Length - 1; index >= 0; index--)
        {
            var digit = digits[index] - '0';
            if (doubleDigit)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            doubleDigit = !doubleDigit;
        }

        return sum % 10 == 0;
    }

    public static bool IsValidExpiry(string? value, DateOnly today)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var match = ExpiryPattern().Match(value.Trim());
        if (!match.Success)
        {
            return false;
        }

        var month = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var year = 2000 + int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        var lastValidDay = new DateOnly(year, month, DateTime.DaysInMonth(year, month));

        return lastValidDay >= today;
    }

    public static bool IsValidCvv(string? value) =>
        value is not null && CvvPattern().IsMatch(value.Trim());

    public static string LastFourDigits(string value)
    {
        var digits = DigitsOnly(value);
        return digits.Length >= 4 ? digits[^4..] : digits;
    }
}
