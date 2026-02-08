using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.BoundControls
{
    public class BoundCheckbox : CheckBox
    {
        public BoundCheckbox() : base() 
        { 
        }

        public void Bind(string propertyName, object dataSource)
        {
            this.DataBindings.Add(new Binding("Checked", dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged));
        }
    }
}
