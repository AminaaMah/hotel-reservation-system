namespace HotelReservation.Core.Models;

public sealed record RoomOption(string Code, string Name, decimal PricePerNight, int Capacity, int Inventory)
{
    public string DisplayText => $"{Name}  ·  up to {Capacity} {(Capacity == 1 ? "guest" : "guests")}  ·  {Money.Format(PricePerNight)} / night";
}

public sealed record BoardOption(string Code, string Name, decimal PricePerNight)
{
    public string DisplayText => $"{Name}  ·  {Money.Format(PricePerNight)} / night";
}

public static class Catalog
{
    public static IReadOnlyList<RoomOption> Rooms { get; } =
    [
        new("SINGLE", "Single room", 50m, 1, 10),
        new("DOUBLE", "Double room", 80m, 2, 10),
        new("TRIPLE", "Triple room", 110m, 3, 6),
        new("DELUXE", "Deluxe room", 150m, 2, 4),
        new("APARTMENT", "Apartment", 250m, 6, 2),
        new("KING", "King apartment", 500m, 12, 1)
    ];

    public static IReadOnlyList<BoardOption> Boards { get; } =
    [
        new("BB", "Breakfast only", 55m),
        new("HB", "Half board", 75m),
        new("FB", "Full board", 90m),
        new("AI", "All inclusive", 120m)
    ];

    public static RoomOption GetRoom(string code) =>
        Rooms.FirstOrDefault(room => room.Code == code)
        ?? throw new ArgumentException($"Unknown room type '{code}'.", nameof(code));

    public static BoardOption GetBoard(string code) =>
        Boards.FirstOrDefault(board => board.Code == code)
        ?? throw new ArgumentException($"Unknown board type '{code}'.", nameof(code));
}
