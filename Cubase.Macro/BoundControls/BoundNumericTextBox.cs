using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.BoundControls
{
    public class BoundNumericTextBox : TextBox
    {
        public void Bind(string propertyName, object dataSource)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Text", dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged));
        }
    }
}
