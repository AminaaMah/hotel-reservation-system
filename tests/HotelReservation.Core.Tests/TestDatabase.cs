using HotelReservation.Core.Data;
using Microsoft.Data.Sqlite;

namespace HotelReservation.Core.Tests;

public abstract class TestDatabase : IDisposable
{
    private readonly string _path = Path.Combine(Path.GetTempPath(), $"hotel-test-{Guid.NewGuid():N}.db");

    protected TestDatabase()
    {
        Database = new HotelDatabase(_path);
        Database.Initialize(seedDemoData: false);
    }

    protected HotelDatabase Database { get; }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }

        GC.SuppressFinalize(this);
    }
}
