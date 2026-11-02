using HotelReservation.App.UI;
using HotelReservation.Core.Data;
using HotelReservation.Core.Validation;

namespace HotelReservation.App.Forms;

internal sealed class RegistrationForm : DialogForm
{
    private readonly AppServices _services;
    private readonly TextBox _firstName = Theme.TextInput(50);
    private readonly TextBox _lastName = Theme.TextInput(50);
    private readonly TextBox _email = Theme.TextInput(150);
    private readonly TextBox _phone = Theme.TextInput(20);
    private readonly TextBox _password = Theme.TextInput(100, isPassword: true);
    private readonly TextBox _confirmPassword = Theme.TextInput(100, isPassword: true);

    public RegistrationForm(AppServices services)
        : base("Create account", "Register once to book and manage your reservations.", 460, 640)
    {
        _services = services;

        AddField("First name", _firstName);
        AddField("Last name", _lastName);
        AddField("Email", _email);
        AddField("Phone", _phone);
        AddField("Password", _password);
        AddContent(Theme.Caption("At least 8 characters, including letters and digits."));
        AddField("Confirm password", _confirmPassword);

        var create = Theme.PrimaryButton("Create account", 150);
        create.Click += (_, _) => Register();

        var cancel = Theme.SecondaryButton("Cancel", 100);
        cancel.DialogResult = DialogResult.Cancel;

        AddButtons(create, cancel);
        AcceptButton = create;
        CancelButton = cancel;
    }

    public string RegisteredEmail { get; private set; } = string.Empty;

    private void Register()
    {
        var errors = new List<string>();

        if (!InputValidator.IsValidName(_firstName.Text))
        {
            errors.Add("Enter your first name.");
        }

        if (!InputValidator.IsValidName(_lastName.Text))
        {
            errors.Add("Enter your last name.");
        }

        if (!InputValidator.IsValidEmail(_email.Text))
        {
            errors.Add("Enter a valid email address.");
        }

        if (!InputValidator.IsValidPhone(_phone.Text))
        {
            errors.Add("Enter a valid phone number, for example +387 61 123 456.");
        }

        if (!InputValidator.IsStrongPassword(_password.Text))
        {
            errors.Add("The password must have at least 8 characters, including letters and digits.");
        }
        else if (_password.Text != _confirmPassword.Text)
        {
            errors.Add("The passwords do not match.");
        }

        if (errors.Count > 0)
        {
            ShowErrors("Create account", errors);
            return;
        }

        try
        {
            var user = _services.Users.Register(_firstName.Text, _lastName.Text, _email.Text, _phone.Text, _password.Text);
            RegisteredEmail = user.Email;

            MessageBox.Show(this, "Your account has been created. You can now sign in.", "Create account", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (DuplicateEmailException exception)
        {
            ShowError("Create account", exception.Message);
        }
    }
}
