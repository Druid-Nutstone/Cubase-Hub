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
            ButtonPositionCubase = new Button();
            ButtonClose = new Button();
            ButtonBack = new Button();
            MainPanel = new Panel();
            NavPanel.SuspendLayout();
            SuspendLayout();
            // 
            // NavPanel
            // 
            NavPanel.Controls.Add(ButtonPositionCubase);
            NavPanel.Controls.Add(ButtonClose);
            NavPanel.Controls.Add(ButtonBack);
            NavPanel.Dock = DockStyle.Top;
            NavPanel.Location = new Point(0, 0);
            NavPanel.Name = "NavPanel";
            NavPanel.Size = new Size(200, 61);
            NavPanel.TabIndex = 0;
            // 
            // ButtonPositionCubase
            // 
            ButtonPositionCubase.BackColor = SystemColors.ControlDarkDark;
            ButtonPositionCubase.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ButtonPositionCubase.ForeColor = SystemColors.ButtonHighlight;
            ButtonPositionCubase.Location = new Point(99, 3);
            ButtonPositionCubase.Name = "ButtonPositionCubase";
            ButtonPositionCubase.Size = new Size(38, 29);
            ButtonPositionCubase.TabIndex = 2;
            ButtonPositionCubase.Text = "C";
            ButtonPositionCubase.UseVisualStyleBackColor = false;
            // 
            // ButtonClose
            // 
            ButtonClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ButtonClose.BackColor = Color.IndianRed;
            ButtonClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ButtonClose.ForeColor = SystemColors.ButtonHighlight;
            ButtonClose.Location = new Point(143, 3);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(45, 29);
            ButtonClose.TabIndex = 1;
            ButtonClose.Text = "X";
            ButtonClose.UseVisualStyleBackColor = false;
            // 
            // ButtonBack
            // 
            ButtonBack.BackColor = SystemColors.ControlDarkDark;
            ButtonBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ButtonBack.ForeColor = SystemColors.ButtonHighlight;
            ButtonBack.Location = new Point(12, 3);
            ButtonBack.Name = "ButtonBack";
            ButtonBack.Size = new Size(46, 29);
            ButtonBack.TabIndex = 0;
            ButtonBack.Text = "<<";
            ButtonBack.UseVisualStyleBackColor = false;
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
        private Button ButtonClose;
        private Button ButtonPositionCubase;
    }
}
