namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    partial class ToggleKeyEditor
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
            MacroToggleOn = new MacroKeySelectorControl();
            label1 = new Label();
            label2 = new Label();
            MacroToggleOff = new MacroKeySelectorControl();
            SuspendLayout();
            // 
            // MacroToggleOn
            // 
            MacroToggleOn.Location = new Point(15, 40);
            MacroToggleOn.Name = "MacroToggleOn";
            MacroToggleOn.Size = new Size(449, 208);
            MacroToggleOn.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(15, 17);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 1;
            label1.Text = "Toggle On";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(483, 17);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 2;
            label2.Text = "Toggle Off";
            // 
            // MacroToggleOff
            // 
            MacroToggleOff.Location = new Point(483, 40);
            MacroToggleOff.Name = "MacroToggleOff";
            MacroToggleOff.Size = new Size(439, 231);
            MacroToggleOff.TabIndex = 3;
            // 
            // ToggleKeyEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(MacroToggleOff);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(MacroToggleOn);
            Name = "ToggleKeyEditor";
            Size = new Size(1004, 315);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MacroKeySelectorControl MacroToggleOn;
        private Label label1;
        private Label label2;
        private MacroKeySelectorControl MacroToggleOff;
    }
}
