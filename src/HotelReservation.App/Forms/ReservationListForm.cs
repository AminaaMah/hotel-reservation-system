using HotelReservation.App.UI;
using HotelReservation.Core.Models;

namespace HotelReservation.App.Forms;

internal sealed class ReservationListForm : Form
{
    private readonly AppServices _services;
    private readonly User? _guest;
    private readonly DataGridView _grid = CreateGrid();
    private readonly CheckBox _showCancelled = new() { Text = "Show cancelled", AutoSize = true, Margin = new Padding(0, 10, 0, 0) };
    private readonly Label _statistics = new() { AutoSize = true, Font = Theme.LabelFont, ForeColor = Theme.Text, Margin = new Padding(0, 10, 24, 0) };

    private ReservationListForm(AppServices services, User? guest, string title, string subtitle)
    {
        _services = services;
        _guest = guest;

        Text = title;
        Font = Theme.BodyFont;
        BackColor = Theme.Background;
        ForeColor = Theme.Text;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MinimizeBox = false;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1040, 580);
        MinimumSize = new Size(820, 420);

        ConfigureColumns();

        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 46,
            Padding = new Padding(20, 4, 20, 0),
            BackColor = Theme.Background
        };
        if (IsAdministrator)
        {
            toolbar.Controls.Add(_statistics);
        }

        toolbar.Controls.Add(_showCancelled);
        _showCancelled.CheckedChanged += (_, _) => LoadReservations();

        var gridHost = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20, 4, 20, 12),
            BackColor = Theme.Background
        };
        gridHost.Controls.Add(_grid);

        var buttonBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 66,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(18, 14, 18, 14),
            BackColor = Theme.Surface
        };

        var close = Theme.SecondaryButton("Close", 100);
        close.DialogResult = DialogResult.Cancel;

        var refresh = Theme.SecondaryButton("Refresh", 100);
        refresh.Click += (_, _) => LoadReservations();

        var cancelReservation = Theme.PrimaryButton("Cancel reservation", 170);
        cancelReservation.Click += (_, _) => CancelSelected();

        buttonBar.Controls.Add(close);
        buttonBar.Controls.Add(refresh);
        buttonBar.Controls.Add(cancelReservation);
        CancelButton = close;

        Controls.Add(gridHost);
        Controls.Add(buttonBar);
        Controls.Add(toolbar);
        Controls.Add(Theme.Header(title, subtitle));

        Load += (_, _) => LoadReservations();
    }

    private bool IsAdministrator => _guest is null;

    public static ReservationListForm ForAdministrator(AppServices services) =>
        new(services, null, "Reservation overview", "All reservations, availability and revenue at a glance.");

    public static ReservationListForm ForGuest(AppServices services, User guest) =>
        new(services, guest, "My reservations", $"Reservations made by {guest.FullName}.");

    private static DataGridView CreateGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false,
            BackgroundColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            GridColor = Theme.Border,
            EnableHeadersVisualStyles = false,
            ColumnHeadersHeight = 38,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        };

        grid.ColumnHeadersDefaultCellStyle.BackColor = Theme.Primary;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = Theme.LabelFont;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Theme.Primary;
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        grid.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        grid.DefaultCellStyle.SelectionBackColor = Theme.Selection;
        grid.DefaultCellStyle.SelectionForeColor = Theme.Text;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Theme.Background;
        grid.RowTemplate.Height = 34;
        return grid;
    }

    private void ConfigureColumns()
    {
        AddColumn("Id", "#", 45);
        if (IsAdministrator)
        {
            AddColumn("Guest", "Guest", 150);
        }

        AddColumn("Room", "Room", 130);
        AddColumn("Board", "Board", 120);
        AddColumn("CheckIn", "Check-in", 105);
        AddColumn("CheckOut", "Check-out", 105);
        AddColumn("Nights", "Nights", 60);
        AddColumn("Guests", "Guests", 60);
        AddColumn("Total", "Total", 95, DataGridViewContentAlignment.MiddleRight);
        AddColumn("Payment", "Payment", 170);
        AddColumn("Status", "Status", 85);
    }

    private void AddColumn(string name, string header, int weight, DataGridViewContentAlignment alignment = DataGridViewContentAlignment.MiddleLeft)
    {
        var column = new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            FillWeight = weight,
            SortMode = DataGridViewColumnSortMode.NotSortable
        };
        column.DefaultCellStyle.Alignment = alignment;
        _grid.Columns.Add(column);
    }

    private void LoadReservations()
    {
        var reservations = _guest is null
            ? _services.Reservations.GetAll(_showCancelled.Checked)
            : _services.Reservations.GetByUser(_guest.Id, _showCancelled.Checked);

        _grid.Rows.Clear();
        foreach (var reservation in reservations)
        {
            var values = new List<object>
            {
                reservation.Id
            };

            if (IsAdministrator)
            {
                values.Add(reservation.GuestName);
            }

            values.Add(reservation.RoomName);
            values.Add(reservation.BoardName);
            values.Add(DateFormat.Display(reservation.CheckIn));
            values.Add(DateFormat.Display(reservation.CheckOut));
            values.Add(reservation.Nights);
            values.Add(reservation.Guests);
            values.Add(Money.Format(reservation.TotalPrice));
            values.Add(reservation.PaymentDescription);
            values.Add(reservation.StatusText);

            var index = _grid.Rows.Add(values.ToArray());
            var row = _grid.Rows[index];
            row.Tag = reservation;

            if (reservation.IsCancelled)
            {
                row.DefaultCellStyle.ForeColor = Theme.MutedText;
            }
        }

        if (IsAdministrator)
        {
            var statistics = _services.Reservations.GetStatistics(AppServices.Today);
            _statistics.Text =
                $"Active: {statistics.ActiveReservations}     Upcoming stays: {statistics.UpcomingStays}     " +
                $"Paid: {Money.Format(statistics.PaidRevenue)}     Outstanding: {Money.Format(statistics.OutstandingRevenue)}";
        }
    }

    private void CancelSelected()
    {
        if (_grid.CurrentRow?.Tag is not Reservation reservation)
        {
            MessageBox.Show(this, "Select a reservation first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (reservation.IsCancelled)
        {
            MessageBox.Show(this, "This reservation is already cancelled.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!IsAdministrator && reservation.CheckIn <= AppServices.Today)
        {
            MessageBox.Show(this, "Stays that have already started can only be cancelled at the reception.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var answer = MessageBox.Show(
            this,
            $"Cancel reservation #{reservation.Id} ({reservation.RoomName}, {DateFormat.Display(reservation.CheckIn)} – {DateFormat.Display(reservation.CheckOut)})?",
            "Cancel reservation",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (answer != DialogResult.Yes)
        {
            return;
        }

        if (!_services.Reservations.Cancel(reservation.Id, _guest?.Id))
        {
            MessageBox.Show(this, "The reservation could not be cancelled. It may have been changed in the meantime.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        LoadReservations();
    }
}
