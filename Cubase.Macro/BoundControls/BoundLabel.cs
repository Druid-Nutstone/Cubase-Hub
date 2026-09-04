namespace Cubase.Macro.BoundControls
{
    public class BoundLabel : Label
    {

        public void Bind(string propertyName, object dataSource)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Text", dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged));
        }

    }
}
