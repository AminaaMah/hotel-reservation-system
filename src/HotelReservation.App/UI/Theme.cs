namespace HotelReservation.App.UI;

internal static class Theme
{
    public static readonly Color Primary = Color.FromArgb(22, 50, 79);
    public static readonly Color PrimaryHover = Color.FromArgb(36, 72, 110);
    public static readonly Color Accent = Color.FromArgb(196, 154, 60);
    public static readonly Color Background = Color.FromArgb(245, 246, 248);
    public static readonly Color Surface = Color.White;
    public static readonly Color Border = Color.FromArgb(218, 223, 230);
    public static readonly Color Text = Color.FromArgb(33, 37, 41);
    public static readonly Color MutedText = Color.FromArgb(104, 112, 122);
    public static readonly Color HeaderSubtext = Color.FromArgb(200, 210, 222);
    public static readonly Color Success = Color.FromArgb(25, 120, 70);
    public static readonly Color Danger = Color.FromArgb(176, 42, 55);
    public static readonly Color Selection = Color.FromArgb(222, 232, 244);

    public static readonly Font BodyFont = new("Segoe UI", 10F);
    public static readonly Font LabelFont = new("Segoe UI Semibold", 9.5F);
    public static readonly Font CaptionFont = new("Segoe UI", 9F);
    public static readonly Font HeadingFont = new("Segoe UI Semibold", 15F);
    public static readonly Font DisplayFont = new("Segoe UI Semibold", 26F);
    public static readonly Font TotalFont = new("Segoe UI Semibold", 14F);

    public static Button PrimaryButton(string text, int width = 150)
    {
        var button = CreateButton(text, width);
        button.BackColor = Primary;
        button.ForeColor = Color.White;
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = PrimaryHover;
        return button;
    }

    public static Button SecondaryButton(string text, int width = 150)
    {
        var button = CreateButton(text, width);
        button.BackColor = Surface;
        button.ForeColor = Primary;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = Border;
        button.FlatAppearance.MouseOverBackColor = Background;
        return button;
    }

    public static Label FieldLabel(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Font = LabelFont,
        ForeColor = Text,
        Margin = new Padding(0, 10, 0, 4)
    };

    public static Label Caption(string text, int maxWidth = 0) => new()
    {
        Text = text,
        AutoSize = true,
        MaximumSize = new Size(maxWidth, 0),
        Font = CaptionFont,
        ForeColor = MutedText,
        Margin = new Padding(0, 6, 0, 4)
    };

    public static TextBox TextInput(int maxLength, bool isPassword = false) => new()
    {
        Font = BodyFont,
        BorderStyle = BorderStyle.FixedSingle,
        MaxLength = maxLength,
        UseSystemPasswordChar = isPassword,
        Anchor = AnchorStyles.Left | AnchorStyles.Right,
        Margin = new Padding(0, 0, 0, 2)
    };

    public static Panel Header(string title, string subtitle)
    {
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 78,
            BackColor = Primary
        };

        var accent = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 3,
            BackColor = Accent
        };

        var titleLabel = new Label
        {
            Text = title,
            AutoSize = true,
            Font = HeadingFont,
            ForeColor = Color.White,
            Location = new Point(26, 12)
        };

        var subtitleLabel = new Label
        {
            Text = subtitle,
            AutoSize = true,
            Font = CaptionFont,
            ForeColor = HeaderSubtext,
            Location = new Point(28, 44)
        };

        header.Controls.Add(titleLabel);
        header.Controls.Add(subtitleLabel);
        header.Controls.Add(accent);
        return header;
    }

    public static void ShowErrors(IWin32Window owner, string title, IEnumerable<string> errors) =>
        MessageBox.Show(
            owner,
            string.Join(Environment.NewLine, errors.Select(error => $"•  {error}")),
            title,
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

    private static Button CreateButton(string text, int width) => new()
    {
        Text = text,
        Font = LabelFont,
        FlatStyle = FlatStyle.Flat,
        Size = new Size(width, 38),
        Cursor = Cursors.Hand,
        Margin = new Padding(10, 0, 0, 0),
        UseVisualStyleBackColor = false
    };
}
