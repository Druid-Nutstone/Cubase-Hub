namespace Cubase.Macro.Mobile.Controls
{
    public class BoundYamlTextBox : Entry
    {
        private string propertyName;

        private object source;

        public BoundYamlTextBox() : base()
        {


        }

        public void Bind(string property, object source)
        {
            this.source = source;
            this.propertyName = property;
            if (source != null)
            {
                this.BindingContext = source;
                // Bind the Entry's Text property to the specified YAML property name
                this.SetBinding(Entry.TextProperty, new Binding(this.propertyName, BindingMode.TwoWay));
            }
        }
    }
}
