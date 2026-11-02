using HotelReservation.App.UI;
using HotelReservation.Core.Models;
using HotelReservation.Core.Services;

namespace HotelReservation.App.Forms;

internal sealed class ReservationSummaryForm : DialogForm
{
    public ReservationSummaryForm(ReservationRequest request, User guest)
        : base("Review reservation", "Check the details before confirming.", 480, 520)
    {
        var details = new TableLayoutPanel
        {
            ColumnCount = 2,
            AutoSize = true,
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Theme.Surface,
            Padding = new Padding(16, 10, 16, 10),
            Margin = new Padding(0, 10, 0, 0)
        };
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        AddDetail(details, "Guest", guest.FullName);
        AddDetail(details, "Room", request.Room.Name);
        AddDetail(details, "Board", request.Board.Name);
        AddDetail(details, "Check-in", DateFormat.Display(request.CheckIn));
        AddDetail(details, "Check-out", DateFormat.Display(request.CheckOut));
        AddDetail(details, "Nights", request.Nights.ToString());
        AddDetail(details, "Guests", DescribeGuests(request.Adults, request.Children));
        AddDetail(details, "Price per night", Money.Format(PriceCalculator.PricePerNight(request.Room, request.Board)));
        AddDetail(details, "Payment", request.PaymentMethod == PaymentMethod.Card ? "Card, paid now" : "At the hotel");

        var total = new Label
        {
            Text = $"Total  {Money.Format(PriceCalculator.Total(request))}",
            AutoSize = true,
            Font = Theme.TotalFont,
            ForeColor = Theme.Primary,
            Margin = new Padding(0, 16, 0, 0)
        };

        AddContent(details);
        AddContent(total);

        var confirm = Theme.PrimaryButton(request.PaymentMethod == PaymentMethod.Card ? "Continue to payment" : "Confirm reservation", 190);
        confirm.DialogResult = DialogResult.OK;

        var back = Theme.SecondaryButton("Back", 100);
        back.DialogResult = DialogResult.Cancel;

        AddButtons(confirm, back);
        AcceptButton = confirm;
        CancelButton = back;
    }

    private static void AddDetail(TableLayoutPanel table, string label, string value)
    {
        table.Controls.Add(new Label { Text = label, AutoSize = true, ForeColor = Theme.MutedText, Margin = new Padding(0, 5, 8, 5) });
        table.Controls.Add(new Label { Text = value, AutoSize = true, Font = Theme.LabelFont, Margin = new Padding(0, 5, 0, 5) });
    }

    private static string DescribeGuests(int adults, int children)
    {
        var text = $"{adults} {(adults == 1 ? "adult" : "adults")}";
        return children == 0 ? text : $"{text}, {children} {(children == 1 ? "child" : "children")}";
    }
}
