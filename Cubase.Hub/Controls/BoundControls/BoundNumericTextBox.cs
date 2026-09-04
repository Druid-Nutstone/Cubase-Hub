using System.ComponentModel;

namespace Cubase.Hub.Controls.BoundControls
{


    public class BoundNumericTextBox : TextBox
    {
        private ToolTip tooltip;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? ToolTipText { get; set; } = null;

        public void Bind(string propertyName, object dataSource, DataSourceUpdateMode propertyUpdateType = DataSourceUpdateMode.OnPropertyChanged, string? toolTipText = null)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Text", dataSource, propertyName, true, propertyUpdateType));
            if (!string.IsNullOrEmpty(toolTipText))
            {
                this.tooltip = new ToolTip();
                this.tooltip.SetToolTip(this, toolTipText);
            }
        }
    }
}
