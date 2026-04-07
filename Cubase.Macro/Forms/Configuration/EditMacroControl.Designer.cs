namespace Cubase.Macro.Forms.Configuration
{
    partial class EditMacroControl
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
            UpdateButton = new Button();
            label1 = new Label();
            MacroTitle = new Cubase.Macro.BoundControls.BoundTextBox();
            panel1 = new Panel();
            panel2 = new Panel();
            ContentPanel = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // UpdateButton
            // 
            UpdateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            UpdateButton.Location = new Point(386, 12);
            UpdateButton.Name = "UpdateButton";
            UpdateButton.Size = new Size(93, 29);
            UpdateButton.TabIndex = 0;
            UpdateButton.Text = "Update";
            UpdateButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 15);
            label1.Name = "label1";
            label1.Size = new Size(38, 20);
            label1.TabIndex = 1;
            label1.Text = "Title";
            // 
            // MacroTitle
            // 
            MacroTitle.Location = new Point(23, 38);
            MacroTitle.Name = "MacroTitle";
            MacroTitle.Size = new Size(173, 27);
            MacroTitle.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(UpdateButton);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 341);
            panel1.Name = "panel1";
            panel1.Size = new Size(494, 55);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(label1);
            panel2.Controls.Add(MacroTitle);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(494, 123);
            panel2.TabIndex = 4;
            // 
            // ContentPanel
            // 
            ContentPanel.Dock = DockStyle.Fill;
            ContentPanel.Location = new Point(0, 123);
            ContentPanel.Name = "ContentPanel";
            ContentPanel.Size = new Size(494, 218);
            ContentPanel.TabIndex = 5;
            // 
            // EditMacroControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ContentPanel);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "EditMacroControl";
            Size = new Size(494, 396);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button UpdateButton;
        private Label label1;
        private BoundControls.BoundTextBox MacroTitle;
        private Panel panel1;
        private Panel panel2;
        private Panel ContentPanel;
    }
}
