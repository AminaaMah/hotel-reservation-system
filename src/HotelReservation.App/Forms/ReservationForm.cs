using HotelReservation.App.UI;
using HotelReservation.Core.Data;
using HotelReservation.Core.Models;
using HotelReservation.Core.Services;

namespace HotelReservation.App.Forms;

internal sealed class ReservationForm : DialogForm
{
    private readonly AppServices _services;
    private readonly User _user;

    private readonly DateTimePicker _checkIn = CreateDatePicker();
    private readonly DateTimePicker _checkOut = CreateDatePicker();
    private readonly NumericUpDown _adults = new() { Minimum = 1, Maximum = 12, Value = 2, Width = 90 };
    private readonly NumericUpDown _children = new() { Minimum = 0, Maximum = 10, Value = 0, Width = 90 };
    private readonly ComboBox _room = CreateComboBox();
    private readonly ComboBox _board = CreateComboBox();
    private readonly RadioButton _payByCard = new() { Text = "Pay now by card", AutoSize = true, Checked = true, Margin = new Padding(0, 4, 18, 0) };
    private readonly RadioButton _payAtHotel = new() { Text = "Pay at the hotel", AutoSize = true, Margin = new Padding(0, 4, 0, 0) };
    private readonly Label _availability = Theme.Caption(string.Empty);
    private readonly Label _breakdown = Theme.Caption(string.Empty);
    private readonly Label _total = new() { AutoSize = true, Font = Theme.TotalFont, ForeColor = Theme.Primary };

    public ReservationForm(AppServices services, User user)
        : base("New reservation", $"Signed in as {user.FullName}", 600, 640)
    {
        _services = services;
        _user = user;

        foreach (var room in Catalog.Rooms)
        {
            _room.Items.Add(room);
        }

        foreach (var board in Catalog.Boards)
        {
            _board.Items.Add(board);
        }

        _room.Format += (_, e) => e.Value = ((RoomOption)e.ListItem!).DisplayText;
        _board.Format += (_, e) => e.Value = ((BoardOption)e.ListItem!).DisplayText;
        _room.SelectedIndex = 1;
        _board.SelectedIndex = 0;

        var today = DateTime.Today;
        _checkIn.MinDate = today;
        _checkIn.Value = today.AddDays(1);
        _checkOut.MinDate = today.AddDays(1);
        _checkOut.Value = today.AddDays(3);

        AddContent(CreateFieldGrid());
        AddContent(_availability);
        AddContent(CreateSummaryPanel());

        var next = Theme.PrimaryButton("Continue", 130);
        next.Click += (_, _) => Continue();

        var myReservations = Theme.SecondaryButton("My reservations", 150);
        myReservations.Click += (_, _) => OpenMyReservations();

        var signOut = Theme.SecondaryButton("Sign out", 100);
        signOut.DialogResult = DialogResult.Cancel;

        AddButtons(next, myReservations, signOut);
        AcceptButton = next;

        _checkIn.ValueChanged += (_, _) => OnCheckInChanged();
        _checkOut.ValueChanged += (_, _) => UpdateSummary();
        _adults.ValueChanged += (_, _) => UpdateSummary();
        _children.ValueChanged += (_, _) => UpdateSummary();
        _room.SelectedIndexChanged += (_, _) => UpdateSummary();
        _board.SelectedIndexChanged += (_, _) => UpdateSummary();

        UpdateSummary();
    }

    private static DateTimePicker CreateDatePicker() => new()
    {
        Format = DateTimePickerFormat.Custom,
        CustomFormat = "dd MMM yyyy",
        Width = 160
    };

    private static ComboBox CreateComboBox() => new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        FormattingEnabled = true,
        Anchor = AnchorStyles.Left | AnchorStyles.Right
    };

    private TableLayoutPanel CreateFieldGrid()
    {
        var grid = new TableLayoutPanel
        {
            ColumnCount = 2,
            AutoSize = true,
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            Margin = new Padding(0, 8, 0, 0)
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        var payment = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0) };
        payment.Controls.Add(_payByCard);
        payment.Controls.Add(_payAtHotel);

        AddRow(grid, "Check-in", _checkIn);
        AddRow(grid, "Check-out", _checkOut);
        AddRow(grid, "Adults", _adults);
        AddRow(grid, "Children", _children);
        AddRow(grid, "Room type", _room);
        AddRow(grid, "Board", _board);
        AddRow(grid, "Payment", payment);
        return grid;
    }

    private static void AddRow(TableLayoutPanel grid, string label, Control input)
    {
        var caption = Theme.FieldLabel(label);
        caption.Anchor = AnchorStyles.Left;
        caption.Margin = new Padding(0, 8, 8, 8);
        input.Margin = new Padding(0, 6, 0, 6);
        if (input is ComboBox)
        {
            input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        }

        grid.Controls.Add(caption);
        grid.Controls.Add(input);
    }

    private Panel CreateSummaryPanel()
    {
        var panel = new Panel
        {
            Height = 92,
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Theme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 10, 0, 0),
            Padding = new Padding(16, 10, 16, 10)
        };

        var title = new Label { Text = "Estimated total", AutoSize = true, Font = Theme.LabelFont, ForeColor = Theme.MutedText, Location = new Point(16, 10) };
        _breakdown.Location = new Point(16, 34);
        _breakdown.Margin = new Padding(0);
        _total.Location = new Point(16, 54);

        panel.Controls.Add(title);
        panel.Controls.Add(_breakdown);
        panel.Controls.Add(_total);
        return panel;
    }

    private void OnCheckInChanged()
    {
        var minimumCheckOut = _checkIn.Value.Date.AddDays(1);
        if (_checkOut.Value.Date < minimumCheckOut)
        {
            _checkOut.Value = minimumCheckOut;
        }

        _checkOut.MinDate = minimumCheckOut;
        UpdateSummary();
    }

    private ReservationRequest BuildRequest() => new(
        _user.Id,
        DateOnly.FromDateTime(_checkIn.Value),
        DateOnly.FromDateTime(_checkOut.Value),
        (int)_adults.Value,
        (int)_children.Value,
        ((RoomOption)_room.SelectedItem!).Code,
        ((BoardOption)_board.SelectedItem!).Code,
        _payByCard.Checked ? PaymentMethod.Card : PaymentMethod.PayAtHotel);

    private void UpdateSummary()
    {
        if (_room.SelectedItem is null || _board.SelectedItem is null)
        {
            return;
        }

        var request = BuildRequest();
        if (request.Nights < 1)
        {
            _breakdown.Text = "Select a check-out date after check-in.";
            _total.Text = "–";
            _availability.Text = string.Empty;
            return;
        }

        var perNight = PriceCalculator.PricePerNight(request.Room, request.Board);
        _breakdown.Text = $"{request.Nights} {(request.Nights == 1 ? "night" : "nights")} × {Money.Format(perNight)}  ({request.Room.Name} + {request.Board.Name.ToLowerInvariant()})";
        _total.Text = Money.Format(PriceCalculator.Total(request));

        var available = _services.Reservations.AvailableRooms(request.RoomCode, request.CheckIn, request.CheckOut);
        if (available > 0)
        {
            _availability.Text = $"{available} of {request.Room.Inventory} rooms of this type are available for the selected dates.";
            _availability.ForeColor = Theme.Success;
        }
        else
        {
            _availability.Text = "This room type is fully booked for the selected dates.";
            _availability.ForeColor = Theme.Danger;
        }
    }

    private void Continue()
    {
        var request = BuildRequest();
        var today = AppServices.Today;

        var errors = _services.Booking.Validate(request, today);
        if (errors.Count > 0)
        {
            ShowErrors("New reservation", errors);
            return;
        }

        using (var summary = new ReservationSummaryForm(request, _user))
        {
            if (summary.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
        }

        string? cardLast4 = null;
        if (request.PaymentMethod == PaymentMethod.Card)
        {
            using var payment = new PaymentForm(PriceCalculator.Total(request));
            if (payment.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            cardLast4 = payment.CardLast4;
        }

        try
        {
            var reservation = _services.Booking.Book(request, today, cardLast4);

            MessageBox.Show(
                this,
                $"Reservation #{reservation.Id} is confirmed.{Environment.NewLine}{Environment.NewLine}" +
                $"{reservation.RoomName}, {DateFormat.Display(reservation.CheckIn)} – {DateFormat.Display(reservation.CheckOut)}{Environment.NewLine}" +
                $"Total: {Money.Format(reservation.TotalPrice)}  ·  {reservation.PaymentDescription}",
                "Reservation confirmed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            UpdateSummary();
        }
        catch (Exception exception) when (exception is RoomUnavailableException or InvalidOperationException)
        {
            ShowError("New reservation", exception.Message);
            UpdateSummary();
        }
    }

    private void OpenMyReservations()
    {
        using var list = ReservationListForm.ForGuest(_services, _user);
        list.ShowDialog(this);
        UpdateSummary();
    }
}
