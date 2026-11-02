using HotelReservation.Core.Data;
using HotelReservation.Core.Models;
using HotelReservation.Core.Services;

namespace HotelReservation.Core.Tests;

public class UserRepositoryTests : TestDatabase
{
    private readonly UserRepository _users;

    public UserRepositoryTests()
    {
        _users = new UserRepository(Database);
    }

    [Fact]
    public void RegisteredUserCanSignIn()
    {
        _users.Register("Ana", "Test", "ana@example.com", "+387 61 123 456", "Secure123");

        var user = Assert.NotNull(_users.Authenticate("ana@example.com", "Secure123"));
        Assert.Equal("Ana Test", user.FullName);
    }

    [Fact]
    public void EmailIsCaseInsensitive()
    {
        _users.Register("Ana", "Test", "ana@example.com", "+387 61 123 456", "Secure123");

        Assert.NotNull(_users.Authenticate("ANA@example.com", "Secure123"));
        Assert.Throws<DuplicateEmailException>(() =>
            _users.Register("Ana", "Other", "Ana@Example.com", "+387 61 123 457", "Secure123"));
    }

    [Fact]
    public void WrongPasswordIsRejected()
    {
        _users.Register("Ana", "Test", "ana@example.com", "+387 61 123 456", "Secure123");

        Assert.Null(_users.Authenticate("ana@example.com", "Wrong123"));
        Assert.Null(_users.Authenticate("nobody@example.com", "Secure123"));
    }

    [Fact]
    public void AdminPasswordIsCheckedAgainstHash()
    {
        var admins = new AdminRepository(Database);
        admins.Create("manager", "Manager123");

        Assert.True(admins.Authenticate("manager", "Manager123"));
        Assert.False(admins.Authenticate("manager", "manager123"));
    }
}

public class ReservationServiceTests : TestDatabase
{
    private static readonly DateOnly Today = new(2026, 11, 2);

    private readonly ReservationRepository _reservations;
    private readonly ReservationService _service;
    private readonly User _guest;

    public ReservationServiceTests()
    {
        _reservations = new ReservationRepository(Database);
        _service = new ReservationService(_reservations);
        _guest = new UserRepository(Database).Register("Ana", "Test", "ana@example.com", "+387 61 123 456", "Secure123");
    }

    private ReservationRequest Request(string room, int checkInOffset, int checkOutOffset, int adults = 1, int children = 0, PaymentMethod payment = PaymentMethod.PayAtHotel) =>
        new(_guest.Id, Today.AddDays(checkInOffset), Today.AddDays(checkOutOffset), adults, children, room, "BB", payment);

    [Fact]
    public void ValidRequestHasNoErrors()
    {
        Assert.Empty(_service.Validate(Request("DOUBLE", 1, 4, adults: 2), Today));
    }

    [Fact]
    public void PastCheckInAndZeroNightsAreRejected()
    {
        var errors = _service.Validate(Request("DOUBLE", -1, -1), Today);

        Assert.Contains(errors, error => error.Contains("past"));
        Assert.Contains(errors, error => error.Contains("at least one day"));
    }

    [Fact]
    public void RoomCapacityIsEnforced()
    {
        var errors = _service.Validate(Request("DOUBLE", 1, 3, adults: 2, children: 1), Today);

        Assert.Contains("up to 2 guests", Assert.Single(errors));
    }

    [Fact]
    public void BookingStoresTotalPriceAndPaymentDetails()
    {
        var reservation = _service.Book(Request("DOUBLE", 1, 4, adults: 2, payment: PaymentMethod.Card), Today, "1111");

        Assert.Equal(405m, reservation.TotalPrice);
        Assert.True(reservation.IsPaid);
        Assert.Equal("1111", reservation.CardLast4);
        Assert.Equal("Ana Test", reservation.GuestName);
    }

    [Fact]
    public void CardPaymentWithoutTransactionIsRejected()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _service.Book(Request("DOUBLE", 1, 4, payment: PaymentMethod.Card), Today, null));
    }

    [Fact]
    public void FullyBookedRoomTypeIsUnavailableForOverlappingDates()
    {
        _service.Book(Request("KING", 5, 8), Today, null);

        Assert.NotEmpty(_service.Validate(Request("KING", 6, 9), Today));
        Assert.NotEmpty(_service.Validate(Request("KING", 3, 6), Today));
        Assert.Throws<RoomUnavailableException>(() =>
            _reservations.Create(Request("KING", 7, 10), 100m, null));
    }

    [Fact]
    public void BackToBackStaysDoNotOverlap()
    {
        _service.Book(Request("KING", 5, 8), Today, null);

        Assert.Empty(_service.Validate(Request("KING", 8, 10), Today));
        Assert.Empty(_service.Validate(Request("KING", 2, 5), Today));
    }

    [Fact]
    public void CancellingFreesTheRoom()
    {
        var reservation = _service.Book(Request("KING", 5, 8), Today, null);

        Assert.True(_reservations.Cancel(reservation.Id));
        Assert.False(_reservations.Cancel(reservation.Id));
        Assert.Empty(_service.Validate(Request("KING", 5, 8), Today));
    }

    [Fact]
    public void GuestCanOnlyCancelOwnReservation()
    {
        var reservation = _service.Book(Request("SINGLE", 1, 2), Today, null);

        Assert.False(_reservations.Cancel(reservation.Id, userId: _guest.Id + 1));
        Assert.True(_reservations.Cancel(reservation.Id, userId: _guest.Id));
    }

    [Fact]
    public void StatisticsSeparatePaidAndOutstandingRevenue()
    {
        _service.Book(Request("SINGLE", 1, 3, payment: PaymentMethod.Card), Today, "1111");
        _service.Book(Request("DOUBLE", 2, 3), Today, null);
        var cancelled = _service.Book(Request("TRIPLE", 2, 3), Today, null);
        _reservations.Cancel(cancelled.Id);

        var statistics = _reservations.GetStatistics(Today);

        Assert.Equal(2, statistics.ActiveReservations);
        Assert.Equal(210m, statistics.PaidRevenue);
        Assert.Equal(135m, statistics.OutstandingRevenue);
        Assert.Equal(2, _reservations.GetAll(includeCancelled: false).Count);
        Assert.Equal(3, _reservations.GetAll(includeCancelled: true).Count);
    }
}

public class DemoDataTests : TestDatabase
{
    [Fact]
    public void DemoDataIsSeededOnlyOnce()
    {
        DemoData.SeedIfEmpty(Database);
        DemoData.SeedIfEmpty(Database);

        var reservations = new ReservationRepository(Database).GetAll(includeCancelled: true);

        Assert.Equal(3, reservations.Count);
        Assert.True(new AdminRepository(Database).Authenticate(DemoData.AdminUsername, DemoData.AdminPassword));
        Assert.NotNull(new UserRepository(Database).Authenticate(DemoData.GuestEmail, DemoData.GuestPassword));
    }
}
