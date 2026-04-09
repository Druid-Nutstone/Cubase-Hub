namespace Cubase.Macro.Forms.Main
{
    partial class MainMenuControl
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
            NavPanel = new Panel();
            MainPanel = new Panel();
            ButtonBack = new Button();
            NavPanel.SuspendLayout();
            SuspendLayout();
            // 
            // NavPanel
            // 
            NavPanel.Controls.Add(ButtonBack);
            NavPanel.Dock = DockStyle.Top;
            NavPanel.Location = new Point(0, 0);
            NavPanel.Name = "NavPanel";
            NavPanel.Size = new Size(200, 61);
            NavPanel.TabIndex = 0;
            // 
            // MainPanel
            // 
            MainPanel.AutoScroll = true;
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 61);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(200, 453);
            MainPanel.TabIndex = 1;
            // 
            // ButtonBack
            // 
            ButtonBack.Location = new Point(14, 17);
            ButtonBack.Name = "ButtonBack";
            ButtonBack.Size = new Size(46, 29);
            ButtonBack.TabIndex = 0;
            ButtonBack.Text = "<<";
            ButtonBack.UseVisualStyleBackColor = true;
            // 
            // MainMenuControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(MainPanel);
            Controls.Add(NavPanel);
            Name = "MainMenuControl";
            Size = new Size(200, 514);
            NavPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel NavPanel;
        private Panel MainPanel;
        private Button ButtonBack;
    }
}
