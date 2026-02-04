namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    partial class CubaseProjectExtendedPropertiesControl
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
            Mixes = new GroupBox();
            SuspendLayout();
            // 
            // Mixes
            // 
            Mixes.AutoSize = true;
            Mixes.Dock = DockStyle.Top;
            Mixes.ForeColor = SystemColors.Control;
            Mixes.Location = new Point(0, 0);
            Mixes.Name = "Mixes";
            Mixes.Size = new Size(400, 5);
            Mixes.TabIndex = 0;
            Mixes.TabStop = false;
            Mixes.Text = "Mixes";
            // 
            // CubaseProjectExtendedPropertiesControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(Mixes);
            Name = "CubaseProjectExtendedPropertiesControl";
            Size = new Size(400, 279);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox Mixes;
    }
}
