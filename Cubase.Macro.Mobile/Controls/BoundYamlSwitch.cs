using System.ComponentModel;

namespace Cubase.Macro.Mobile.Controls
{
    public class BoundYamlSwitch : Microsoft.Maui.Controls.Switch
    {
        private string propertyName;
        private INotifyPropertyChanged source;

        public BoundYamlSwitch()
        {
        }

        private void BoundYamlSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            var prop = this.source.GetType().GetProperty(this.propertyName);
            if (prop == null) return;

            // Get the target property type (handles nullable types too)
            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            // Safely convert the bool toggle value to whatever type the property expects
            var convertedValue = Convert.ChangeType(e.Value, targetType);
            prop.SetValue(this.source, convertedValue);
        }

        public void Bind(string property, INotifyPropertyChanged source)
        {
            if (source == null)
                return;

            this.source = source;
            this.propertyName = property;

            var prop = this.source.GetType().GetProperty(propertyName);
            if (prop != null)
            {
                var val = prop.GetValue(this.source);
                if (val != null)
                {
                    // Safely convert whatever is stored in the model into a boolean for the switch
                    this.IsToggled = Convert.ToBoolean(val);
                }
            }

            this.Toggled += BoundYamlSwitch_Toggled;
        }
    }
}