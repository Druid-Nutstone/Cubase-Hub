namespace Cubase.Macro.Mobile.Controls
{
    public class BaseImageButton : ImageButton
    {
        public BaseImageButton() : base()
        {
            this.Pressed += BaseButton_Pressed;
            this.Released += BaseButton_Released;
        }

        private async void BaseButton_Released(object? sender, EventArgs e)
        {
            await this.ScaleToAsync(1, 100);

        }

        private async void BaseButton_Pressed(object? sender, EventArgs e)
        {
            await this.ScaleToAsync(0.85, 100);
        }
    }
}
