namespace Cubase.Macro.Models
{
    public class DropDownToggleItemCollection : List<DropDownToggleItem>
    {
    }

    public class DropDownToggleItem
    {
        public string ActiveText { get; set; }

        public string? InActiveText { get; set; }

        public string IconText { get; set; }

        public Func<bool, bool> OnClick { get; set; }

        public Color? ActiveBackColour { get; set; }

        public Color? ActiveTextColour { get; set; }

        public Color? InActiveBackColour { get; set; }

        public Color? InActiveTextColour { get; set; }

        public DropDownToggleItemCollection? SubItems { get; set; }

    }
}
