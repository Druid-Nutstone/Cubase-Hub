using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Cues.CueControls
{
    public class ToggleCubaseButton : CheckBox
    {

        private Color checkColour = Color.Empty;

        private Color defaultBack = Color.Empty;
        private Color defaultFore = Color.Empty;

        public ToggleCubaseButton() : base() 
        {
            this.Size = new System.Drawing.Size(30, 30);
            this.Appearance = Appearance.Button;
            this.CheckAlign = ContentAlignment.MiddleCenter;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.AutoSize = false;
            this.Font = new Font(this.Font, FontStyle.Bold);
            this.Text = "X";
            this.defaultBack = this.BackColor;
            this.defaultFore = this.ForeColor;
        }

        public void Bind(string propertyName, object dataSource, Color checkColor, DataSourceUpdateMode propertyUpdateType = DataSourceUpdateMode.OnPropertyChanged)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Checked", dataSource, propertyName, true, propertyUpdateType));
            this.checkColour = checkColor;
            this.SetColours();
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            SetColours();
        }

        private void SetColours()
        {
            this.BackColor = this.Checked ? this.checkColour : this.defaultBack;
            this.ForeColor = this.Checked ? Color.Black : this.defaultFore;
        }
    }
}
