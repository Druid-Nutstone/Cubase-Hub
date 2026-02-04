using Cubase.Hub.Forms.BaseForm;
using System.Drawing;
using System.Windows.Forms;

public class DarkToolStripRenderer : ToolStripProfessionalRenderer
{
    public DarkToolStripRenderer()
        : base(new DarkColorTable()) { }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

        Color backColor = e.Item.Selected
            ? DarkTheme.ControlColor
            : DarkTheme.PanelColor;

        using var brush = new SolidBrush(backColor);
        e.Graphics.FillRectangle(brush, rect);
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        e.TextColor = DarkTheme.TextColor; // 🔑 THIS FIXES BLACK TEXT
        base.OnRenderItemText(e);
    }
}


public class DarkColorTable : ProfessionalColorTable
{
    public override Color MenuItemSelected => DarkTheme.ControlColor;
    public override Color MenuItemBorder => DarkTheme.BorderColor;
    public override Color MenuBorder => DarkTheme.BorderColor;

    public override Color ToolStripDropDownBackground => DarkTheme.PanelColor;
    public override Color ImageMarginGradientBegin => DarkTheme.PanelColor;
    public override Color ImageMarginGradientMiddle => DarkTheme.PanelColor;
    public override Color ImageMarginGradientEnd => DarkTheme.PanelColor;
}
