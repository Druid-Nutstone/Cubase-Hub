namespace Cubase.Macro.Forms.Configuration
{
    partial class SettingsMainControl
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
            TreePanel = new Panel();
            panel2 = new Panel();
            NewMenu = new LinkLabel();
            splitter1 = new Splitter();
            DataPanel = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(TreePanel);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 541);
            panel1.TabIndex = 0;
            // 
            // TreePanel
            // 
            TreePanel.Dock = DockStyle.Fill;
            TreePanel.Location = new Point(0, 72);
            TreePanel.Name = "TreePanel";
            TreePanel.Size = new Size(250, 469);
            TreePanel.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(NewMenu);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(250, 72);
            panel2.TabIndex = 0;
            // 
            // NewMenu
            // 
            NewMenu.AutoSize = true;
            NewMenu.Location = new Point(14, 24);
            NewMenu.Name = "NewMenu";
            NewMenu.Size = new Size(80, 20);
            NewMenu.TabIndex = 0;
            NewMenu.TabStop = true;
            NewMenu.Text = "New Menu";
            // 
            // splitter1
            // 
            splitter1.Location = new Point(250, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(4, 541);
            splitter1.TabIndex = 1;
            splitter1.TabStop = false;
            // 
            // DataPanel
            // 
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(254, 0);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(469, 541);
            DataPanel.TabIndex = 2;
            // 
            // SettingsMainControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DataPanel);
            Controls.Add(splitter1);
            Controls.Add(panel1);
            Name = "SettingsMainControl";
            Size = new Size(723, 541);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel TreePanel;
        private Panel panel2;
        private LinkLabel NewMenu;
        private Splitter splitter1;
        private Panel DataPanel;
    }
}
