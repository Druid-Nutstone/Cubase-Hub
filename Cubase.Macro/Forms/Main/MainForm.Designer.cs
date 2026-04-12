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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DataPanel = new Panel();
            mainMenuControl = new Cubase.Macro.Forms.Main.MainMenuControl();
            DataPanel.SuspendLayout();
            SuspendLayout();
            // 
            // DataPanel
            // 
            DataPanel.Controls.Add(mainMenuControl);
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(0, 0);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(192, 450);
            DataPanel.TabIndex = 1;
            // 
            // mainMenuControl
            // 
            mainMenuControl.Dock = DockStyle.Fill;
            mainMenuControl.Location = new Point(0, 0);
            mainMenuControl.Name = "mainMenuControl";
            mainMenuControl.Size = new Size(192, 450);
            mainMenuControl.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(192, 450);
            Controls.Add(DataPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            DataPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel DataPanel;
        private Forms.Main.MainMenuControl mainMenuControl;
    }
}