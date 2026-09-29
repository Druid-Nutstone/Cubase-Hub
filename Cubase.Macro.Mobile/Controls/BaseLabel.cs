namespace Cubase.Macro.Mobile.Controls
{
    public class BaseLabel : Label
    {
        private readonly TapGestureRecognizer _tapGesture;

        public Func<Label, Task> OnTapped { get; set; }

        public BaseLabel() : base()
        {
            _tapGesture = new TapGestureRecognizer();
            this.GestureRecognizers.Add(this._tapGesture);
            _tapGesture.Tapped += _tapGesture_Tapped;

        }

        private async void _tapGesture_Tapped(object? sender, TappedEventArgs e)
        {
            await this.ScaleToAsync(0.90, 60);
            await this.OnTapped?.Invoke(this);
            await this.ScaleToAsync(1.0, 60);
        }
    }
}
