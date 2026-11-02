using HotelReservation.App.UI;
using HotelReservation.Core.Models;
using HotelReservation.Core.Validation;

namespace HotelReservation.App.Forms;

internal sealed class PaymentForm : DialogForm
{
    private readonly TextBox _cardholder = Theme.TextInput(60);
    private readonly TextBox _cardNumber = Theme.TextInput(23);
    private readonly TextBox _expiry = Theme.TextInput(5);
    private readonly TextBox _cvv = Theme.TextInput(4, isPassword: true);

    public PaymentForm(decimal amount)
        : base("Card payment", $"Amount due: {Money.Format(amount)}", 440, 560)
    {
        AddField("Cardholder name", _cardholder);
        AddField("Card number", _cardNumber);
        AddField("Expiry date (MM/YY)", _expiry);
        AddField("Security code (CVV)", _cvv);

        AddContent(Theme.Caption(
            "Demo payment: no money is charged. Card details are validated but never stored – only the last four digits are saved with the reservation. Test card: 4242 4242 4242 4242.",
            370));

        var pay = Theme.PrimaryButton($"Pay {Money.Format(amount)}", 160);
        pay.Click += (_, _) => Pay();

        var cancel = Theme.SecondaryButton("Cancel", 100);
        cancel.DialogResult = DialogResult.Cancel;

        AddButtons(pay, cancel);
        AcceptButton = pay;
        CancelButton = cancel;
    }

    public string CardLast4 { get; private set; } = string.Empty;

    private void Pay()
    {
        var errors = new List<string>();

        if (!InputValidator.IsValidName(_cardholder.Text))
        {
            errors.Add("Enter the cardholder name.");
        }

        if (!CardValidator.IsValidNumber(_cardNumber.Text))
        {
            errors.Add("The card number is not valid.");
        }

        if (!CardValidator.IsValidExpiry(_expiry.Text, AppServices.Today))
        {
            errors.Add("Enter a valid expiry date in the format MM/YY.");
        }

        if (!CardValidator.IsValidCvv(_cvv.Text))
        {
            errors.Add("The security code must have 3 or 4 digits.");
        }

        if (errors.Count > 0)
        {
            ShowErrors("Card payment", errors);
            return;
        }

        CardLast4 = CardValidator.LastFourDigits(_cardNumber.Text);
        _cardNumber.Clear();
        _cvv.Clear();
        DialogResult = DialogResult.OK;
    }
}
