namespace Cubase.Macro.Forms.Configuration.ColourPicker
{
    partial class ColourPickerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TitleLabel = new Label();
            ButtonColour = new Button();
            SuspendLayout();
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            TitleLabel.Location = new Point(13, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(77, 20);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "TitleLabel";
            // 
            // ButtonColour
            // 
            ButtonColour.Location = new Point(13, 23);
            ButtonColour.Name = "ButtonColour";
            ButtonColour.Size = new Size(94, 29);
            ButtonColour.TabIndex = 1;
            ButtonColour.Text = "Pick Colour";
            ButtonColour.UseVisualStyleBackColor = true;
            // 
            // ColourPickerControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ButtonColour);
            Controls.Add(TitleLabel);
            Name = "ColourPickerControl";
            Size = new Size(199, 60);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label TitleLabel;
        private Button ButtonColour;
    }
}
