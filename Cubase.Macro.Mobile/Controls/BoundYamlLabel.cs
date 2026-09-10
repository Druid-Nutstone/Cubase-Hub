namespace Cubase.Macro.Mobile.Controls
{
    public class BoundYamlLabel : Label
    {
        public BoundYamlLabel(string property, object source) : base()
        {
            this.Text = source?.GetType()?.GetProperty(property)?.GetValue(source) as string;
        }
    }
}
