namespace Cubase.Macro.BoundControls
{
    public class BoundNumericUpDown : NumericUpDown
    {
        public BoundNumericUpDown() : base()
        {
        }

        public void Bind(string propertyName, object dataSource)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Value", dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged));
        }
    }
}
