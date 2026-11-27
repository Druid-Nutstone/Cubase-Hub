namespace Cubase.Macro.Forms.Scripts
{
    partial class EditScriptForm
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
            MainMenu = new MenuStrip();
            MainPanel = new Panel();
            SuspendLayout();
            // 
            // MainMenu
            // 
            MainMenu.ImageScalingSize = new Size(20, 20);
            MainMenu.Location = new Point(0, 0);
            MainMenu.Name = "MainMenu";
            MainMenu.Size = new Size(800, 28);
            MainMenu.TabIndex = 0;
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 28);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(800, 422);
            MainPanel.TabIndex = 1;
            // 
            // EditScriptForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MainPanel);
            Controls.Add(MainMenu);
            MainMenuStrip = MainMenu;
            Name = "EditScriptForm";
            Text = "NewScriptForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip MainMenu;
        private Panel MainPanel;
    }
}