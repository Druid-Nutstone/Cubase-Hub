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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuControl));
            NavPanel = new Panel();
            ButtonMinimise = new Cubase.Macro.Forms.Main.Buttons.PictureButton();
            ButtonClose = new Cubase.Macro.Forms.Main.Buttons.PictureButton();
            ButtonPositionCubase = new Button();
            MainPanel = new Panel();
            ButtonBack = new Cubase.Macro.Forms.Main.Buttons.PictureButton();
            NavPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ButtonMinimise).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ButtonClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ButtonBack).BeginInit();
            SuspendLayout();
            // 
            // NavPanel
            // 
            NavPanel.Controls.Add(ButtonBack);
            NavPanel.Controls.Add(ButtonMinimise);
            NavPanel.Controls.Add(ButtonClose);
            NavPanel.Controls.Add(ButtonPositionCubase);
            NavPanel.Dock = DockStyle.Top;
            NavPanel.Location = new Point(0, 0);
            NavPanel.Name = "NavPanel";
            NavPanel.Size = new Size(200, 61);
            NavPanel.TabIndex = 0;
            // 
            // ButtonMinimise
            // 
            ButtonMinimise.Image = (Image)resources.GetObject("ButtonMinimise.Image");
            ButtonMinimise.Location = new Point(113, 3);
            ButtonMinimise.Name = "ButtonMinimise";
            ButtonMinimise.Size = new Size(32, 32);
            ButtonMinimise.SizeMode = PictureBoxSizeMode.StretchImage;
            ButtonMinimise.TabIndex = 7;
            ButtonMinimise.TabStop = false;
            // 
            // ButtonClose
            // 
            ButtonClose.Image = Properties.Resources.icons8_close_window_32;
            ButtonClose.Location = new Point(151, 3);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(32, 32);
            ButtonClose.SizeMode = PictureBoxSizeMode.StretchImage;
            ButtonClose.TabIndex = 6;
            ButtonClose.TabStop = false;
            // 
            // ButtonPositionCubase
            // 
            ButtonPositionCubase.BackColor = SystemColors.ControlDarkDark;
            ButtonPositionCubase.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ButtonPositionCubase.ForeColor = SystemColors.ButtonHighlight;
            ButtonPositionCubase.Location = new Point(56, 3);
            ButtonPositionCubase.Name = "ButtonPositionCubase";
            ButtonPositionCubase.Size = new Size(38, 29);
            ButtonPositionCubase.TabIndex = 2;
            ButtonPositionCubase.Text = "C";
            ButtonPositionCubase.UseVisualStyleBackColor = false;
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
            ButtonBack.Image = (Image)resources.GetObject("ButtonBack.Image");
            ButtonBack.Location = new Point(3, 3);
            ButtonBack.Name = "ButtonBack";
            ButtonBack.Size = new Size(32, 32);
            ButtonBack.SizeMode = PictureBoxSizeMode.StretchImage;
            ButtonBack.TabIndex = 8;
            ButtonBack.TabStop = false;
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
            ((System.ComponentModel.ISupportInitialize)ButtonMinimise).EndInit();
            ((System.ComponentModel.ISupportInitialize)ButtonClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)ButtonBack).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel NavPanel;
        private Panel MainPanel;
        private Button ButtonPositionCubase;
        private Buttons.PictureButton ButtonClose;
        private Buttons.PictureButton ButtonMinimise;
        private Buttons.PictureButton ButtonBack;
    }
}
