using HotelReservation.App.UI;
using HotelReservation.Core.Data;
using HotelReservation.Core.Models;

namespace HotelReservation.App.Forms;

internal sealed class LoginForm : DialogForm
{
    private readonly AppServices _services;
    private readonly TextBox _email = Theme.TextInput(150);
    private readonly TextBox _password = Theme.TextInput(100, isPassword: true);

    public LoginForm(AppServices services)
        : base("Guest sign in", "Sign in to book a room or manage your stays.", 440, 420)
    {
        _services = services;

        AddField("Email", _email);
        AddField("Password", _password);

        var registerLink = new LinkLabel
        {
            Text = "New guest? Create an account",
            AutoSize = true,
            LinkColor = Theme.Primary,
            ActiveLinkColor = Theme.Accent,
            Margin = new Padding(0, 14, 0, 0)
        };
        registerLink.LinkClicked += (_, _) => OpenRegistration();
        AddContent(registerLink);

        AddContent(Theme.Caption($"Demo account: {DemoData.GuestEmail}  /  {DemoData.GuestPassword}"));

        var signIn = Theme.PrimaryButton("Sign in", 120);
        signIn.Click += (_, _) => SignIn();

        var cancel = Theme.SecondaryButton("Cancel", 100);
        cancel.DialogResult = DialogResult.Cancel;

        AddButtons(signIn, cancel);
        AcceptButton = signIn;
        CancelButton = cancel;
    }

    public User? SignedInUser { get; private set; }

    private void SignIn()
    {
        if (string.IsNullOrWhiteSpace(_email.Text) || string.IsNullOrEmpty(_password.Text))
        {
            ShowError("Sign in", "Please enter your email and password.");
            return;
        }

        var user = _services.Users.Authenticate(_email.Text, _password.Text);
        if (user is null)
        {
            ShowError("Sign in", "The email or password is incorrect.");
            _password.Clear();
            _password.Focus();
            return;
        }

        SignedInUser = user;
        DialogResult = DialogResult.OK;
    }

    private void OpenRegistration()
    {
        using var registration = new RegistrationForm(_services);
        if (registration.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _email.Text = registration.RegisteredEmail;
        _password.Clear();
        _password.Focus();
    }
}

internal sealed class AdminLoginForm : DialogForm
{
    private readonly AppServices _services;
    private readonly TextBox _username = Theme.TextInput(50);
    private readonly TextBox _password = Theme.TextInput(100, isPassword: true);

    public AdminLoginForm(AppServices services)
        : base("Administrator sign in", "Manage reservations and view revenue.", 420, 360)
    {
        _services = services;

        AddField("Username", _username);
        AddField("Password", _password);
        AddContent(Theme.Caption($"Demo account: {DemoData.AdminUsername}  /  {DemoData.AdminPassword}"));

        var signIn = Theme.PrimaryButton("Sign in", 120);
        signIn.Click += (_, _) => SignIn();

        var cancel = Theme.SecondaryButton("Cancel", 100);
        cancel.DialogResult = DialogResult.Cancel;

        AddButtons(signIn, cancel);
        AcceptButton = signIn;
        CancelButton = cancel;
    }

    private void SignIn()
    {
        if (!_services.Admins.Authenticate(_username.Text, _password.Text))
        {
            ShowError("Administrator sign in", "The username or password is incorrect.");
            _password.Clear();
            _password.Focus();
            return;
        }

        DialogResult = DialogResult.OK;
    }
}
