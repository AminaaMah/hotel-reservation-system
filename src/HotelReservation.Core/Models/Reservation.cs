namespace HotelReservation.Core.Models;

public enum PaymentMethod
{
    Card,
    PayAtHotel
}

public sealed record User(int Id, string FirstName, string LastName, string Email, string Phone)
{
    public string FullName => $"{FirstName} {LastName}";
}

public sealed record ReservationRequest(
    int UserId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Adults,
    int Children,
    string RoomCode,
    string BoardCode,
    PaymentMethod PaymentMethod)
{
    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;

    public int Guests => Adults + Children;

    public RoomOption Room => Catalog.GetRoom(RoomCode);

    public BoardOption Board => Catalog.GetBoard(BoardCode);
}

public sealed record Reservation(
    int Id,
    int UserId,
    string GuestName,
    string GuestEmail,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Adults,
    int Children,
    string RoomCode,
    string BoardCode,
    PaymentMethod PaymentMethod,
    bool IsPaid,
    string? CardLast4,
    decimal TotalPrice,
    bool IsCancelled,
    DateTime CreatedAt)
{
    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;

    public int Guests => Adults + Children;

    public string RoomName => Catalog.GetRoom(RoomCode).Name;

    public string BoardName => Catalog.GetBoard(BoardCode).Name;

    public string PaymentDescription => PaymentMethod == PaymentMethod.Card
        ? $"Paid by card •••• {CardLast4}"
        : "Pay at hotel";

    public string StatusText => IsCancelled ? "Cancelled" : "Active";
}

public sealed record ReservationStatistics(int ActiveReservations, int UpcomingStays, decimal PaidRevenue, decimal OutstandingRevenue);
