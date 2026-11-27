namespace Cubase.Macro.Forms.Scripts
{
    partial class ScriptMainForm
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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            OpenScript = new ToolStripMenuItem();
            ControlPanel = new Panel();
            NewScript = new Button();
            MainPanel = new Panel();
            menuStrip1.SuspendLayout();
            ControlPanel.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenScript });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // OpenScript
            // 
            OpenScript.Name = "OpenScript";
            OpenScript.Size = new Size(224, 26);
            OpenScript.Text = "Open";
            // 
            // ControlPanel
            // 
            ControlPanel.Controls.Add(NewScript);
            ControlPanel.Dock = DockStyle.Top;
            ControlPanel.Location = new Point(0, 28);
            ControlPanel.Name = "ControlPanel";
            ControlPanel.Size = new Size(800, 44);
            ControlPanel.TabIndex = 1;
            // 
            // NewScript
            // 
            NewScript.Location = new Point(12, 9);
            NewScript.Name = "NewScript";
            NewScript.Size = new Size(94, 29);
            NewScript.TabIndex = 0;
            NewScript.Text = "New Script";
            NewScript.UseVisualStyleBackColor = true;
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 72);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(800, 378);
            MainPanel.TabIndex = 2;
            // 
            // ScriptMainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MainPanel);
            Controls.Add(ControlPanel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "ScriptMainForm";
            Text = "ScriptMainForm";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ControlPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private Panel ControlPanel;
        private Panel MainPanel;
        private Button NewScript;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem OpenScript;
    }
}