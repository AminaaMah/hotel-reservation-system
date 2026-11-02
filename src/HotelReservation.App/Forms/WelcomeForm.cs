using HotelReservation.App.UI;

namespace HotelReservation.App.Forms;

internal sealed class WelcomeForm : Form
{
    private readonly AppServices _services;

    public WelcomeForm(AppServices services)
    {
        _services = services;

        Text = "Hotel Horizon – Reservations";
        Font = Theme.BodyFont;
        BackColor = Theme.Surface;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(780, 440);

        Controls.Add(CreateActions());
        Controls.Add(CreateBrandPanel());
    }

    private static Panel CreateBrandPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 330,
            BackColor = Theme.Primary
        };

        var name = new Label
        {
            Text = "Hotel Horizon",
            AutoSize = true,
            Font = Theme.DisplayFont,
            ForeColor = Color.White,
            Location = new Point(40, 140)
        };

        var accent = new Panel
        {
            BackColor = Theme.Accent,
            Size = new Size(60, 4),
            Location = new Point(44, 196)
        };

        var tagline = new Label
        {
            Text = "Book your stay in a few clicks.",
            AutoSize = true,
            Font = Theme.BodyFont,
            ForeColor = Theme.HeaderSubtext,
            Location = new Point(42, 214)
        };

        panel.Controls.Add(name);
        panel.Controls.Add(accent);
        panel.Controls.Add(tagline);
        return panel;
    }

    private TableLayoutPanel CreateActions()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            Padding = new Padding(56, 100, 56, 30),
            BackColor = Theme.Surface
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        var heading = new Label
        {
            Text = "Welcome",
            AutoSize = true,
            Font = Theme.HeadingFont,
            Margin = new Padding(0, 0, 0, 4)
        };

        var question = Theme.Caption("How would you like to continue?");
        question.Margin = new Padding(0, 0, 0, 24);

        var guestButton = Theme.PrimaryButton("Continue as guest", 300);
        guestButton.Height = 46;
        guestButton.Margin = new Padding(0, 0, 0, 12);
        guestButton.Click += (_, _) => OpenGuestArea();

        var adminButton = Theme.SecondaryButton("Administrator sign in", 300);
        adminButton.Height = 46;
        adminButton.Margin = new Padding(0, 0, 0, 12);
        adminButton.Click += (_, _) => OpenAdminArea();

        var exitLink = new LinkLabel
        {
            Text = "Exit",
            AutoSize = true,
            LinkColor = Theme.MutedText,
            ActiveLinkColor = Theme.Primary,
            LinkBehavior = LinkBehavior.HoverUnderline,
            Margin = new Padding(0, 8, 0, 0)
        };
        exitLink.LinkClicked += (_, _) => Close();

        layout.Controls.Add(heading);
        layout.Controls.Add(question);
        layout.Controls.Add(guestButton);
        layout.Controls.Add(adminButton);
        layout.Controls.Add(exitLink);
        return layout;
    }

    private void OpenGuestArea()
    {
        using var login = new LoginForm(_services);
        if (login.ShowDialog(this) != DialogResult.OK || login.SignedInUser is null)
        {
            return;
        }

        using var reservation = new ReservationForm(_services, login.SignedInUser);
        reservation.ShowDialog(this);
    }

    private void OpenAdminArea()
    {
        using var login = new AdminLoginForm(_services);
        if (login.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        using var dashboard = ReservationListForm.ForAdministrator(_services);
        dashboard.ShowDialog(this);
    }
}
