using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Printing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration.ColourPicker
{
    public partial class ColourPickerControl : UserControl
    {
        private ColorDialog colourDialog;

        private Action<Color> onColourChanged;

        private Color currentColour;

        public ColourPickerControl()
        {
            InitializeComponent();
            ButtonColour.Click += ButtonColour_Click;

        }

        private void ButtonColour_Click(object? sender, EventArgs e)
        {
            var colourDialog = new ColorDialog();
            colourDialog.Color = this.currentColour;
            if (colourDialog.ShowDialog() == DialogResult.OK)
            {
                this.currentColour = colourDialog.Color;
                this.onColourChanged?.Invoke(this.currentColour);
            }
        }

        public void Bind(Color colour, string title, Action<Color> onColourChanged)
        {
            this.onColourChanged = onColourChanged;
            this.currentColour = colour;
            this.TitleLabel.Text = title;
        }
    }
}
