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
            MacroButtonType = new Cubase.Macro.BoundControls.BoundEnumComboBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            ContentPanel = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // UpdateButton
            // 
            UpdateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            UpdateButton.Location = new Point(575, 12);
            UpdateButton.Name = "UpdateButton";
            UpdateButton.Size = new Size(93, 29);
            UpdateButton.TabIndex = 0;
            UpdateButton.Text = "Update";
            UpdateButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(23, 15);
            label1.Name = "label1";
            label1.Size = new Size(40, 20);
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
            panel1.Location = new Point(0, 436);
            panel1.Name = "panel1";
            panel1.Size = new Size(683, 55);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(MacroButtonType);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(MacroTitle);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(683, 262);
            panel2.TabIndex = 4;
            // 
            // MacroButtonType
            // 
            MacroButtonType.FormattingEnabled = true;
            MacroButtonType.Location = new Point(227, 38);
            MacroButtonType.Name = "MacroButtonType";
            MacroButtonType.Size = new Size(151, 28);
            MacroButtonType.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(227, 15);
            label4.Name = "label4";
            label4.Size = new Size(95, 20);
            label4.TabIndex = 5;
            label4.Text = "Button Type";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(244, 88);
            label3.Name = "label3";
            label3.Size = new Size(142, 20);
            label3.TabIndex = 4;
            label3.Text = "Button Text Colour";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(23, 88);
            label2.Name = "label2";
            label2.Size = new Size(196, 20);
            label2.TabIndex = 3;
            label2.Text = "Button Background Colour";
            // 
            // ContentPanel
            // 
            ContentPanel.Dock = DockStyle.Fill;
            ContentPanel.Location = new Point(0, 262);
            ContentPanel.Name = "ContentPanel";
            ContentPanel.Size = new Size(683, 174);
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
            Size = new Size(683, 491);
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
        private Label label3;
        private Label label2;
        private BoundControls.BoundEnumComboBox MacroButtonType;
        private Label label4;
    }
}
