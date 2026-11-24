namespace Cubase.Macro.Forms.Configuration.KeyEditors.ConditionalKeys
{
    partial class ConditionalEditorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            OK = new Button();
            panel2 = new Panel();
            panel3 = new Panel();
            EditorControl = new ConditionalKeyEditorControl();
            MacroName = new TextBox();
            label1 = new Label();
            SelectMacroButton = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(OK);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 299);
            panel1.Name = "panel1";
            panel1.Size = new Size(542, 56);
            panel1.TabIndex = 0;
            // 
            // OK
            // 
            OK.Location = new Point(436, 15);
            OK.Name = "OK";
            OK.Size = new Size(94, 29);
            OK.TabIndex = 0;
            OK.Text = "OK";
            OK.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(SelectMacroButton);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(MacroName);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 233);
            panel2.Name = "panel2";
            panel2.Size = new Size(542, 66);
            panel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(EditorControl);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(542, 233);
            panel3.TabIndex = 2;
            // 
            // EditorControl
            // 
            EditorControl.Dock = DockStyle.Fill;
            EditorControl.Location = new Point(0, 0);
            EditorControl.Name = "EditorControl";
            EditorControl.Size = new Size(542, 233);
            EditorControl.TabIndex = 0;
            // 
            // MacroName
            // 
            MacroName.Location = new Point(12, 33);
            MacroName.Name = "MacroName";
            MacroName.ReadOnly = true;
            MacroName.Size = new Size(335, 27);
            MacroName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(12, 10);
            label1.Name = "label1";
            label1.Size = new Size(132, 20);
            label1.TabIndex = 1;
            label1.Text = "Macro To Execute";
            // 
            // SelectMacroButton
            // 
            SelectMacroButton.Location = new Point(378, 32);
            SelectMacroButton.Name = "SelectMacroButton";
            SelectMacroButton.Size = new Size(152, 29);
            SelectMacroButton.TabIndex = 2;
            SelectMacroButton.Text = "Select Macro";
            SelectMacroButton.UseVisualStyleBackColor = true;
            // 
            // ConditionalEditorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(542, 355);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ConditionalEditorForm";
            Text = "ConditionalEditorForm";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button OK;
        private Panel panel2;
        private Panel panel3;
        private ConditionalKeyEditorControl EditorControl;
        private Button SelectMacroButton;
        private Label label1;
        private TextBox MacroName;
    }
}