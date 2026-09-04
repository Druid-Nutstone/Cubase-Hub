namespace Cubase.Hub.Controls.BoundControls
{
    public class BoundComboBox : ComboBox
    {
        public BoundComboBox()
        {

        }

        public void Bind(string propertyName, object dataSource)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("SelectedItem", dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged));
        }
    }
}
