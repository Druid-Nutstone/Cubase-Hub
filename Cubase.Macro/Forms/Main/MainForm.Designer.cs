namespace Cubase.Macro
{
    partial class MainForm
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
            Menu = new MenuStrip();
            DataPanel = new Panel();
            mainMenuControl = new Cubase.Macro.Forms.Main.MainMenuControl();
            DataPanel.SuspendLayout();
            SuspendLayout();
            // 
            // Menu
            // 
            Menu.ImageScalingSize = new Size(20, 20);
            Menu.Location = new Point(0, 0);
            Menu.Name = "Menu";
            Menu.Size = new Size(192, 24);
            Menu.TabIndex = 0;
            Menu.Text = "menuStrip1";
            // 
            // DataPanel
            // 
            DataPanel.Controls.Add(mainMenuControl);
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(0, 24);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(192, 426);
            DataPanel.TabIndex = 1;
            // 
            // mainMenuControl
            // 
            mainMenuControl.Dock = DockStyle.Fill;
            mainMenuControl.Location = new Point(0, 0);
            mainMenuControl.Name = "mainMenuControl";
            mainMenuControl.Size = new Size(192, 426);
            mainMenuControl.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(192, 450);
            Controls.Add(DataPanel);
            Controls.Add(Menu);
            MainMenuStrip = Menu;
            Name = "MainForm";
            Text = "MainForm";
            DataPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip Menu;
        private Panel DataPanel;
        private Forms.Main.MainMenuControl mainMenuControl;
    }
}