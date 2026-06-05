using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Cues.CueControls
{
    public class ToggleCubaseButton : Button
    {

        private Color onBackgroundColour = Color.Empty;
        private Color offBackgroundColour = Color.Empty;
        private Color onForegroundColour = Color.Empty;
        private Color offForegroundColour = Color.Empty;

        private Color defaultBack = Color.Empty;
        
        private HelpProvider helpProvider = new HelpProvider();

        private Action<bool> OnClicked;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OnOff {  get; set; }

        public ToggleCubaseButton() : base() 
        {
            this.Size = new System.Drawing.Size(30, 30);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Font = new Font(this.Font, FontStyle.Bold);
            this.Text = "X";
            this.defaultBack = this.BackColor;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }


        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.OnOff = !this.OnOff;
            this.OnClicked?.Invoke(this.OnOff);
            this.SetColours();
        }

        public void Bind(string propertyName, object dataSource, Color onBackgroundColour, Color offBackgroundColour, Color onForeGroundColour, Color offForegroundColour, Action<bool> onClicked, string helpText)
        {
            this.onBackgroundColour = onBackgroundColour;
            this.offBackgroundColour = offBackgroundColour;
            this.onForegroundColour = onForeGroundColour;
            this.offForegroundColour = offForegroundColour;
            this.OnClicked = onClicked;
            this.helpProvider.SetHelpString(this, helpText);
            this.OnOff = (bool)dataSource.GetType().GetProperty(propertyName).GetValue(dataSource);
            this.SetColours();
        }

        private void SetColours()
        {
            this.BackColor = this.OnOff  ? this.onBackgroundColour : this.offBackgroundColour;
            this.ForeColor = this.OnOff  ? this.onForegroundColour : this.offForegroundColour;
        }
    }
}
