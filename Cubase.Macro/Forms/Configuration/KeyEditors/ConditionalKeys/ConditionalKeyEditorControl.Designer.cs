namespace Cubase.Macro.Forms.Configuration.KeyEditors.ConditionalKeys
{
    partial class ConditionalKeyEditorControl
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
            panel1 = new Panel();
            MainPanel = new Panel();
            AddConditionButton = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(AddConditionButton);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(475, 62);
            panel1.TabIndex = 0;
            // 
            // MainPanel
            // 
            MainPanel.AutoScroll = true;
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 62);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(475, 185);
            MainPanel.TabIndex = 1;
            // 
            // AddConditionButton
            // 
            AddConditionButton.Location = new Point(15, 18);
            AddConditionButton.Name = "AddConditionButton";
            AddConditionButton.Size = new Size(134, 29);
            AddConditionButton.TabIndex = 0;
            AddConditionButton.Text = "Add Condition";
            AddConditionButton.UseVisualStyleBackColor = true;
            // 
            // ConditionalKeyEditorControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(MainPanel);
            Controls.Add(panel1);
            Name = "ConditionalKeyEditorControl";
            Size = new Size(475, 247);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel MainPanel;
        private Button AddConditionButton;
    }
}
