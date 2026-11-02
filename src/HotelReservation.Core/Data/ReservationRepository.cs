using System.Globalization;
using HotelReservation.Core.Models;
using Microsoft.Data.Sqlite;

namespace HotelReservation.Core.Data;

public sealed class RoomUnavailableException(string roomName)
    : Exception($"No {roomName.ToLowerInvariant()} is available for the selected dates.");

public sealed class ReservationRepository(HotelDatabase database)
{
    private const string SelectReservations = """
        SELECT r.Id, r.UserId, u.FirstName || ' ' || u.LastName, u.Email,
               r.CheckIn, r.CheckOut, r.Adults, r.Children, r.RoomCode, r.BoardCode,
               r.PaymentMethod, r.PaymentStatus, r.CardLast4, r.TotalPrice, r.Status, r.CreatedAt
        FROM Reservations r
        INNER JOIN Users u ON u.Id = r.UserId
        """;

    public int AvailableRooms(string roomCode, DateOnly checkIn, DateOnly checkOut)
    {
        using var connection = database.OpenConnection();
        return AvailableRooms(connection, null, roomCode, checkIn, checkOut);
    }

    public Reservation Create(ReservationRequest request, decimal totalPrice, string? cardLast4)
    {
        using var connection = database.OpenConnection();
        using var transaction = connection.BeginTransaction();

        if (AvailableRooms(connection, transaction, request.RoomCode, request.CheckIn, request.CheckOut) < 1)
        {
            throw new RoomUnavailableException(request.Room.Name);
        }

        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            INSERT INTO Reservations
                (UserId, CheckIn, CheckOut, Adults, Children, RoomCode, BoardCode,
                 PaymentMethod, PaymentStatus, CardLast4, TotalPrice)
            VALUES
                ($userId, $checkIn, $checkOut, $adults, $children, $roomCode, $boardCode,
                 $paymentMethod, $paymentStatus, $cardLast4, $totalPrice)
            RETURNING Id;
            """;
        command.Parameters.AddWithValue("$userId", request.UserId);
        command.Parameters.AddWithValue("$checkIn", DateFormat.ToStorage(request.CheckIn));
        command.Parameters.AddWithValue("$checkOut", DateFormat.ToStorage(request.CheckOut));
        command.Parameters.AddWithValue("$adults", request.Adults);
        command.Parameters.AddWithValue("$children", request.Children);
        command.Parameters.AddWithValue("$roomCode", request.RoomCode);
        command.Parameters.AddWithValue("$boardCode", request.BoardCode);
        command.Parameters.AddWithValue("$paymentMethod", request.PaymentMethod.ToString());
        command.Parameters.AddWithValue("$paymentStatus", request.PaymentMethod == PaymentMethod.Card ? "Paid" : "Unpaid");
        command.Parameters.AddWithValue("$cardLast4", (object?)cardLast4 ?? DBNull.Value);
        command.Parameters.AddWithValue("$totalPrice", totalPrice);

        var id = Convert.ToInt32(command.ExecuteScalar());
        transaction.Commit();

        return GetById(id) ?? throw new InvalidOperationException("The reservation could not be loaded after saving.");
    }

    public Reservation? GetById(int id)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"{SelectReservations} WHERE r.Id = $id;";
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public IReadOnlyList<Reservation> GetAll(bool includeCancelled)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            {SelectReservations}
            WHERE $includeCancelled = 1 OR r.Status = 'Active'
            ORDER BY r.CheckIn, r.Id;
            """;
        command.Parameters.AddWithValue("$includeCancelled", includeCancelled ? 1 : 0);
        return ReadAll(command);
    }

    public IReadOnlyList<Reservation> GetByUser(int userId, bool includeCancelled)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            {SelectReservations}
            WHERE r.UserId = $userId AND ($includeCancelled = 1 OR r.Status = 'Active')
            ORDER BY r.CheckIn DESC, r.Id DESC;
            """;
        command.Parameters.AddWithValue("$userId", userId);
        command.Parameters.AddWithValue("$includeCancelled", includeCancelled ? 1 : 0);
        return ReadAll(command);
    }

    public bool Cancel(int reservationId, int? userId = null)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Reservations
            SET Status = 'Cancelled'
            WHERE Id = $id AND Status = 'Active' AND ($userId IS NULL OR UserId = $userId);
            """;
        command.Parameters.AddWithValue("$id", reservationId);
        command.Parameters.AddWithValue("$userId", (object?)userId ?? DBNull.Value);
        return command.ExecuteNonQuery() == 1;
    }

    public ReservationStatistics GetStatistics(DateOnly today)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT
                COUNT(*),
                COALESCE(SUM(CASE WHEN CheckIn >= $today THEN 1 ELSE 0 END), 0),
                COALESCE(SUM(CASE WHEN PaymentStatus = 'Paid' THEN TotalPrice ELSE 0 END), 0),
                COALESCE(SUM(CASE WHEN PaymentStatus = 'Unpaid' THEN TotalPrice ELSE 0 END), 0)
            FROM Reservations
            WHERE Status = 'Active';
            """;
        command.Parameters.AddWithValue("$today", DateFormat.ToStorage(today));

        using var reader = command.ExecuteReader();
        reader.Read();
        return new ReservationStatistics(reader.GetInt32(0), reader.GetInt32(1), reader.GetDecimal(2), reader.GetDecimal(3));
    }

    private static int AvailableRooms(SqliteConnection connection, SqliteTransaction? transaction, string roomCode, DateOnly checkIn, DateOnly checkOut)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            SELECT COUNT(*)
            FROM Reservations
            WHERE RoomCode = $roomCode
              AND Status = 'Active'
              AND CheckIn < $checkOut
              AND CheckOut > $checkIn;
            """;
        command.Parameters.AddWithValue("$roomCode", roomCode);
        command.Parameters.AddWithValue("$checkIn", DateFormat.ToStorage(checkIn));
        command.Parameters.AddWithValue("$checkOut", DateFormat.ToStorage(checkOut));

        var booked = Convert.ToInt32(command.ExecuteScalar());
        return Math.Max(0, Catalog.GetRoom(roomCode).Inventory - booked);
    }

    private static IReadOnlyList<Reservation> ReadAll(SqliteCommand command)
    {
        var reservations = new List<Reservation>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            reservations.Add(Map(reader));
        }

        return reservations;
    }

    private static Reservation Map(SqliteDataReader reader) => new(
        Id: reader.GetInt32(0),
        UserId: reader.GetInt32(1),
        GuestName: reader.GetString(2),
        GuestEmail: reader.GetString(3),
        CheckIn: DateFormat.FromStorage(reader.GetString(4)),
        CheckOut: DateFormat.FromStorage(reader.GetString(5)),
        Adults: reader.GetInt32(6),
        Children: reader.GetInt32(7),
        RoomCode: reader.GetString(8),
        BoardCode: reader.GetString(9),
        PaymentMethod: Enum.Parse<PaymentMethod>(reader.GetString(10)),
        IsPaid: reader.GetString(11) == "Paid",
        CardLast4: reader.IsDBNull(12) ? null : reader.GetString(12),
        TotalPrice: reader.GetDecimal(13),
        IsCancelled: reader.GetString(14) == "Cancelled",
        CreatedAt: DateTime.Parse(reader.GetString(15), CultureInfo.InvariantCulture));
}
