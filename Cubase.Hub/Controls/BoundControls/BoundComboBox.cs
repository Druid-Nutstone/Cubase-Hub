using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.BoundControls
{
    public class BoundComboBox : ComboBox
    {
        public BoundComboBox()
        {

        }

        public void Bind(string propertyName, object dataSource)
        {
            this.DataBindings.Add(new Binding("SelectedItem", dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged));
        }
    }
}
