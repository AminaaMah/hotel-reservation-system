using System.Globalization;

namespace HotelReservation.Core.Models;

public static class Money
{
    public static string Format(decimal amount) =>
        string.Format(CultureInfo.InvariantCulture, "€{0:N2}", amount);
}

public static class DateFormat
{
    public const string Storage = "yyyy-MM-dd";

    public static string Display(DateOnly date) =>
        date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);

    public static string ToStorage(DateOnly date) =>
        date.ToString(Storage, CultureInfo.InvariantCulture);

    public static DateOnly FromStorage(string value) =>
        DateOnly.ParseExact(value, Storage, CultureInfo.InvariantCulture);
}
