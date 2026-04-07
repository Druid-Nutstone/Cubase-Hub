using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.BoundControls
{
    public class BoundCheckbox : CheckBox
    {
        public BoundCheckbox() : base() 
        { 
        }

        public void Bind(string propertyName, object dataSource)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Checked", dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged));
        }
    }
}
