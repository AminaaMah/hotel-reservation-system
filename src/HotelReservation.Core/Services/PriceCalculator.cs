using HotelReservation.Core.Models;

namespace HotelReservation.Core.Services;

public static class PriceCalculator
{
    public static decimal PricePerNight(RoomOption room, BoardOption board) =>
        room.PricePerNight + board.PricePerNight;

    public static decimal Total(RoomOption room, BoardOption board, int nights)
    {
        if (nights < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(nights), "A stay must last at least one night.");
        }

        return PricePerNight(room, board) * nights;
    }

    public static decimal Total(ReservationRequest request) =>
        Total(request.Room, request.Board, request.Nights);
}
