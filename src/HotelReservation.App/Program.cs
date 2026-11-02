using HotelReservation.App.Forms;
using HotelReservation.Core.Data;

namespace HotelReservation.App;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var databasePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "HotelReservation",
            "hotel.db");

        var database = new HotelDatabase(databasePath);

        try
        {
            database.Initialize();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"The database could not be opened.{Environment.NewLine}{Environment.NewLine}{exception.Message}",
                "Hotel Horizon",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        Application.Run(new WelcomeForm(new AppServices(database)));
    }
}
