using Microsoft.Data.Sqlite;

namespace HotelReservation.Core.Data;

public sealed class HotelDatabase
{
    private readonly string _connectionString;

    public HotelDatabase(string databasePath)
    {
        DatabasePath = Path.GetFullPath(databasePath);
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = DatabasePath,
            ForeignKeys = true
        }.ToString();
    }

    public string DatabasePath { get; }

    public SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public void Initialize(bool seedDemoData = true)
    {
        var directory = Path.GetDirectoryName(DatabasePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (var connection = OpenConnection())
        using (var command = connection.CreateCommand())
        {
            command.CommandText = ReadSchema();
            command.ExecuteNonQuery();
        }

        if (seedDemoData)
        {
            DemoData.SeedIfEmpty(this);
        }
    }

    private static string ReadSchema()
    {
        using var stream = typeof(HotelDatabase).Assembly.GetManifestResourceStream("schema.sql")
            ?? throw new InvalidOperationException("The database schema resource is missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
