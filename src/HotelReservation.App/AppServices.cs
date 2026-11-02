using HotelReservation.Core.Data;
using HotelReservation.Core.Services;

namespace HotelReservation.App;

internal sealed class AppServices
{
    public AppServices(HotelDatabase database)
    {
        Users = new UserRepository(database);
        Admins = new AdminRepository(database);
        Reservations = new ReservationRepository(database);
        Booking = new ReservationService(Reservations);
    }

    public UserRepository Users { get; }

    public AdminRepository Admins { get; }

    public ReservationRepository Reservations { get; }

    public ReservationService Booking { get; }

    public static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}
