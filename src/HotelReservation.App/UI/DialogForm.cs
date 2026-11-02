namespace HotelReservation.App.UI;

internal abstract class DialogForm : Form
{
    protected DialogForm(string title, string subtitle, int width, int height)
    {
        Text = title;
        Font = Theme.BodyFont;
        BackColor = Theme.Background;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(width, height);

        Body = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            AutoScroll = true,
            Padding = new Padding(28, 14, 28, 10),
            BackColor = Theme.Background
        };
        Body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        ButtonBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 66,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(18, 14, 18, 14),
            BackColor = Theme.Surface
        };

        Controls.Add(Body);
        Controls.Add(ButtonBar);
        Controls.Add(Theme.Header(title, subtitle));
    }

    protected TableLayoutPanel Body { get; }

    protected FlowLayoutPanel ButtonBar { get; }

    protected void AddField(string label, Control input)
    {
        Body.Controls.Add(Theme.FieldLabel(label));
        input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        Body.Controls.Add(input);
    }

    protected void AddContent(Control control) => Body.Controls.Add(control);

    protected void AddButtons(params Button[] buttonsFromRight)
    {
        foreach (var button in buttonsFromRight)
        {
            ButtonBar.Controls.Add(button);
        }
    }

    protected void ShowErrors(string title, IEnumerable<string> errors) =>
        Theme.ShowErrors(this, title, errors);

    protected void ShowError(string title, string message) =>
        MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
