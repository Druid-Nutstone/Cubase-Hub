using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cubase.Hub.Controls.BoundControls
{
    public class BoundTextBox : TextBox
    {
        private ToolTip tooltip;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? ToolTipText { get; set; } = null;

        public BoundTextBox() : base()
        {

        }

        public void Bind(string propertyName, object dataSource, DataSourceUpdateMode propertyUpdateType = DataSourceUpdateMode.OnPropertyChanged)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Text", dataSource, propertyName, true, propertyUpdateType));
            if (!string.IsNullOrEmpty(this.ToolTipText))
            {
                this.tooltip = new ToolTip();
                this.tooltip.SetToolTip(this,this.ToolTipText);
            }
        }
    }
}
