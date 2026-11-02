using HotelReservation.Core.Models;
using HotelReservation.Core.Services;

namespace HotelReservation.Core.Data;

public static class DemoData
{
    public const string AdminUsername = "admin";
    public const string AdminPassword = "Admin123!";
    public const string GuestEmail = "guest@demo.test";
    public const string GuestPassword = "Guest123!";

    public static void SeedIfEmpty(HotelDatabase database)
    {
        using (var connection = database.OpenConnection())
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT (SELECT COUNT(*) FROM Admins) + (SELECT COUNT(*) FROM Users);";
            if (Convert.ToInt32(command.ExecuteScalar()) > 0)
            {
                return;
            }
        }

        new AdminRepository(database).Create(AdminUsername, AdminPassword);

        var users = new UserRepository(database);
        var demoGuest = users.Register("Demo", "Guest", GuestEmail, "+387 60 000 0001", GuestPassword);
        var sampleGuest = users.Register("Sample", "Traveller", "traveller@demo.test", "+387 60 000 0002", "Sample123!");

        var reservations = new ReservationRepository(database);
        var today = DateOnly.FromDateTime(DateTime.Today);

        Book(reservations, new ReservationRequest(demoGuest.Id, today.AddDays(7), today.AddDays(10), 2, 0, "DOUBLE", "HB", PaymentMethod.Card), "4242");
        Book(reservations, new ReservationRequest(sampleGuest.Id, today.AddDays(14), today.AddDays(21), 2, 2, "APARTMENT", "AI", PaymentMethod.PayAtHotel), null);
        Book(reservations, new ReservationRequest(sampleGuest.Id, today.AddDays(3), today.AddDays(5), 1, 0, "SINGLE", "BB", PaymentMethod.Card), "1881");
    }

    private static void Book(ReservationRepository reservations, ReservationRequest request, string? cardLast4) =>
        reservations.Create(request, PriceCalculator.Total(request), cardLast4);
}
