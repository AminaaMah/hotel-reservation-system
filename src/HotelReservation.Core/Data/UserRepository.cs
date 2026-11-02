using HotelReservation.Core.Models;
using HotelReservation.Core.Security;
using Microsoft.Data.Sqlite;

namespace HotelReservation.Core.Data;

public sealed class DuplicateEmailException(string email)
    : Exception($"An account with the email address {email} already exists.");

public sealed class UserRepository(HotelDatabase database)
{
    private const int SqliteConstraintError = 19;

    public User Register(string firstName, string lastName, string email, string phone, string password)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Users (FirstName, LastName, Email, Phone, PasswordHash)
            VALUES ($firstName, $lastName, $email, $phone, $passwordHash)
            RETURNING Id;
            """;
        command.Parameters.AddWithValue("$firstName", firstName.Trim());
        command.Parameters.AddWithValue("$lastName", lastName.Trim());
        command.Parameters.AddWithValue("$email", email.Trim());
        command.Parameters.AddWithValue("$phone", phone.Trim());
        command.Parameters.AddWithValue("$passwordHash", PasswordHasher.Hash(password));

        try
        {
            var id = Convert.ToInt32(command.ExecuteScalar());
            return new User(id, firstName.Trim(), lastName.Trim(), email.Trim(), phone.Trim());
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == SqliteConstraintError)
        {
            throw new DuplicateEmailException(email.Trim());
        }
    }

    public User? Authenticate(string email, string password)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, FirstName, LastName, Email, Phone, PasswordHash
            FROM Users
            WHERE Email = $email;
            """;
        command.Parameters.AddWithValue("$email", email.Trim());

        using var reader = command.ExecuteReader();
        if (!reader.Read() || !PasswordHasher.Verify(password, reader.GetString(5)))
        {
            return null;
        }

        return new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
    }
}

public sealed class AdminRepository(HotelDatabase database)
{
    public bool Authenticate(string username, string password)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT PasswordHash FROM Admins WHERE Username = $username;";
        command.Parameters.AddWithValue("$username", username.Trim());

        return command.ExecuteScalar() is string hash && PasswordHasher.Verify(password, hash);
    }

    public void Create(string username, string password)
    {
        using var connection = database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Admins (Username, PasswordHash) VALUES ($username, $passwordHash);";
        command.Parameters.AddWithValue("$username", username.Trim());
        command.Parameters.AddWithValue("$passwordHash", PasswordHasher.Hash(password));
        command.ExecuteNonQuery();
    }
}
