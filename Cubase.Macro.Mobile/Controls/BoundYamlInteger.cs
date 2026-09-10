using System.ComponentModel;

namespace Cubase.Macro.Mobile.Controls
{
    public class BoundYamlInteger : Entry
    {
        private string propertyName;

        private INotifyPropertyChanged source;

        public BoundYamlInteger() : base() { }


        public void Bind(string property, INotifyPropertyChanged source)
        {
            this.source = source;
            this.propertyName = property;

            this.source.PropertyChanged += Source_PropertyChanged;

            if (source != null)
            {
                this.Text = ((int)this.source.GetType().GetProperty(this.propertyName).GetValue(source)).ToString();
            }
        }

        private void Source_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.propertyName)
            {
                this.Text = ((int)this.source.GetType().GetProperty(e.PropertyName).GetValue(this.source)).ToString();
            }
        }

        protected override void OnTextChanged(string oldValue, string newValue)
        {
            this.source.GetType().GetProperty(this.propertyName).SetValue(this.source, int.Parse(this.Text));
            base.OnTextChanged(oldValue, newValue);


        }

    }
}
