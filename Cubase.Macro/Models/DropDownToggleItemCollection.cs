namespace Cubase.Macro.Models
{
    public class DropDownToggleItemCollection : List<DropDownToggleItem>
    {


        public DropDownToggleItem AddToggleButton(string activeText,
                                                  string iconText,
                                                  Func<bool, bool> onClick,
                                                  string? inActiveText = null,
                                                  Color? activeBackColour = null,
                                                  Color? activeTextColour = null,
                                                  Color? inActiveBackColour = null,
                                                  Color? InActiveTextColour = null)
        {
            var newButton = new DropDownToggleItem()
            {
                ActiveText = activeText,
                InActiveText = inActiveText,
                IconText = iconText,
                OnClick = onClick,
                ActiveBackColour = activeBackColour,
                ActiveTextColour = activeTextColour,
                InActiveBackColour = inActiveBackColour,
                InActiveTextColour = InActiveTextColour
            };
            this.Add(newButton);
            return newButton;
        }



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

        public DropDownToggleItemCollection? SubItems { get; set; } = new DropDownToggleItemCollection();


        public DropDownToggleItem AddItem(string activeText,
                                                  string iconText,
                                                  Func<bool, bool> onClick,
                                                  string? inActiveText = null,
                                                  Color? activeBackColour = null,
                                                  Color? activeTextColour = null,
                                                  Color? inActiveBackColour = null,
                                                  Color? InActiveTextColour = null)
        {
            var newButton = new DropDownToggleItem()
            {
                ActiveText = activeText,
                InActiveText = inActiveText,
                IconText = iconText,
                OnClick = onClick,
                ActiveBackColour = activeBackColour,
                ActiveTextColour = activeTextColour,
                InActiveBackColour = inActiveBackColour,
                InActiveTextColour = InActiveTextColour
            };
            this.SubItems.Add(newButton);
            return newButton;
        }
    }
}
