using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.BoundControls
{
    public class BoundTextBox : TextBox
    {
        public BoundTextBox() : base()
        {

        }

        public void Bind(string propertyName, object dataSource, DataSourceUpdateMode propertyUpdateType = DataSourceUpdateMode.OnPropertyChanged)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Text", dataSource, propertyName, true, propertyUpdateType));
        }
    }
}
