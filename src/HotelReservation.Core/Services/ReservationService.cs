using HotelReservation.Core.Data;
using HotelReservation.Core.Models;

namespace HotelReservation.Core.Services;

public sealed class ReservationService(ReservationRepository reservations)
{
    public const int MaxNights = 30;

    public IReadOnlyList<string> Validate(ReservationRequest request, DateOnly today)
    {
        var errors = new List<string>();
        var room = request.Room;

        if (request.CheckIn < today)
        {
            errors.Add("The check-in date cannot be in the past.");
        }

        if (request.Nights < 1)
        {
            errors.Add("The check-out date must be at least one day after check-in.");
        }
        else if (request.Nights > MaxNights)
        {
            errors.Add($"Stays longer than {MaxNights} nights must be arranged directly with the hotel.");
        }

        if (request.Adults < 1)
        {
            errors.Add("At least one adult must be included in the reservation.");
        }

        if (request.Children < 0)
        {
            errors.Add("The number of children cannot be negative.");
        }

        if (request.Guests > room.Capacity)
        {
            errors.Add($"The {room.Name.ToLowerInvariant()} accommodates up to {room.Capacity} {(room.Capacity == 1 ? "guest" : "guests")}.");
        }

        if (errors.Count == 0 && reservations.AvailableRooms(request.RoomCode, request.CheckIn, request.CheckOut) < 1)
        {
            errors.Add($"No {room.Name.ToLowerInvariant()} is available for the selected dates.");
        }

        return errors;
    }

    public Reservation Book(ReservationRequest request, DateOnly today, string? cardLast4)
    {
        var errors = Validate(request, today);
        if (errors.Count > 0)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, errors));
        }

        if (request.PaymentMethod == PaymentMethod.Card && string.IsNullOrEmpty(cardLast4))
        {
            throw new InvalidOperationException("Card payments require a completed card transaction.");
        }

        var paymentReference = request.PaymentMethod == PaymentMethod.Card ? cardLast4 : null;
        return reservations.Create(request, PriceCalculator.Total(request), paymentReference);
    }
}
