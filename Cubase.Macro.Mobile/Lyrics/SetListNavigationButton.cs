using Cubase.Macro.Mobile.Controls;

namespace Cubase.Macro.Mobile.Lyrics
{
    public class SetListNavigationButton : BaseButton
    {
        private Action<SetListDirection> OnSetlistNavigation;

        private SetListDirection direction;

        public SetListNavigationButton(Action<SetListDirection> onSetListNavigation, SetListDirection setListDirection) : base()
        {
            this.OnSetlistNavigation = onSetListNavigation;
            this.Text = setListDirection == SetListDirection.Back ? "◄" : "►"; // Solid triangles render more reliably across platforms than unicode arrows
            this.FontSize = 20;
            this.direction = setListDirection;
            this.FontAttributes = FontAttributes.Bold;
            this.BackgroundColor = Color.FromArgb("#2e2e2e");
            this.WidthRequest = 44; // Standard touch target minimum for mobile
            this.HeightRequest = 44;
            this.HorizontalOptions = LayoutOptions.Center;
            this.VerticalOptions = LayoutOptions.Center;
            this.Padding = new Thickness(0); // Fixes Android text offset bugs
            this.TextColor = Colors.Green;
            this.Clicked += SetListNavigationButton_Clicked;
        }

        private void SetListNavigationButton_Clicked(object? sender, EventArgs e)
        {
            this.OnSetlistNavigation?.Invoke(this.direction);
        }
    }

    public enum SetListDirection
    {
        Back,
        Forward,
    }
}
