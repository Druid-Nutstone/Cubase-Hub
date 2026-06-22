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
            ButtonLyrics = new Cubase.Macro.Forms.Main.Buttons.ActionButton();
            ButtonCueLevels = new Cubase.Macro.Forms.Main.Buttons.ActionButton();
            ButtonReload = new Cubase.Macro.Forms.Main.Buttons.ActionButton();
            ButtonRefresh = new Cubase.Macro.Forms.Main.Buttons.PictureButton();
            ButtonPositionCubase = new Cubase.Macro.Forms.Main.Buttons.PictureButton();
            ButtonBack = new Cubase.Macro.Forms.Main.Buttons.PictureButton();
            ButtonMinimise = new Cubase.Macro.Forms.Main.Buttons.PictureButton();
            ButtonClose = new Cubase.Macro.Forms.Main.Buttons.PictureButton();
            MainPanel = new Panel();
            MenuSplitter = new Splitter();
            CommonPanel = new Panel();
            NavPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ButtonRefresh).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ButtonPositionCubase).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ButtonBack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ButtonMinimise).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ButtonClose).BeginInit();
            SuspendLayout();
            // 
            // NavPanel
            // 
            NavPanel.Controls.Add(ButtonLyrics);
            NavPanel.Controls.Add(ButtonCueLevels);
            NavPanel.Controls.Add(ButtonReload);
            NavPanel.Controls.Add(ButtonRefresh);
            NavPanel.Controls.Add(ButtonPositionCubase);
            NavPanel.Controls.Add(ButtonBack);
            NavPanel.Controls.Add(ButtonMinimise);
            NavPanel.Controls.Add(ButtonClose);
            NavPanel.Dock = DockStyle.Top;
            NavPanel.Location = new Point(0, 0);
            NavPanel.Name = "NavPanel";
            NavPanel.Size = new Size(200, 79);
            NavPanel.TabIndex = 0;
            // 
            // ButtonLyrics
            // 
            ButtonLyrics.FlatAppearance.BorderSize = 0;
            ButtonLyrics.FlatStyle = FlatStyle.Flat;
            ButtonLyrics.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ButtonLyrics.Location = new Point(83, 40);
            ButtonLyrics.Name = "ButtonLyrics";
            ButtonLyrics.Size = new Size(30, 30);
            ButtonLyrics.TabIndex = 14;
            ButtonLyrics.Text = "L";
            ButtonLyrics.UseVisualStyleBackColor = true;
            // 
            // ButtonCueLevels
            // 
            ButtonCueLevels.FlatAppearance.BorderSize = 0;
            ButtonCueLevels.FlatStyle = FlatStyle.Flat;
            ButtonCueLevels.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ButtonCueLevels.Location = new Point(43, 40);
            ButtonCueLevels.Name = "ButtonCueLevels";
            ButtonCueLevels.Size = new Size(30, 30);
            ButtonCueLevels.TabIndex = 13;
            ButtonCueLevels.Text = "C";
            ButtonCueLevels.UseVisualStyleBackColor = true;
            // 
            // ButtonReload
            // 
            ButtonReload.FlatAppearance.BorderSize = 0;
            ButtonReload.FlatStyle = FlatStyle.Flat;
            ButtonReload.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ButtonReload.Location = new Point(2, 40);
            ButtonReload.Name = "ButtonReload";
            ButtonReload.Size = new Size(30, 30);
            ButtonReload.TabIndex = 12;
            ButtonReload.Text = "R";
            ButtonReload.UseVisualStyleBackColor = true;
            // 
            // ButtonRefresh
            // 
            ButtonRefresh.Image = (Image)resources.GetObject("ButtonRefresh.Image");
            ButtonRefresh.Location = new Point(44, 3);
            ButtonRefresh.Name = "ButtonRefresh";
            ButtonRefresh.Size = new Size(32, 32);
            ButtonRefresh.SizeMode = PictureBoxSizeMode.StretchImage;
            ButtonRefresh.TabIndex = 10;
            ButtonRefresh.TabStop = false;
            // 
            // ButtonPositionCubase
            // 
            ButtonPositionCubase.Image = (Image)resources.GetObject("ButtonPositionCubase.Image");
            ButtonPositionCubase.Location = new Point(81, 3);
            ButtonPositionCubase.Name = "ButtonPositionCubase";
            ButtonPositionCubase.Size = new Size(32, 32);
            ButtonPositionCubase.SizeMode = PictureBoxSizeMode.StretchImage;
            ButtonPositionCubase.TabIndex = 9;
            ButtonPositionCubase.TabStop = false;
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
            // ButtonMinimise
            // 
            ButtonMinimise.Image = (Image)resources.GetObject("ButtonMinimise.Image");
            ButtonMinimise.Location = new Point(118, 3);
            ButtonMinimise.Name = "ButtonMinimise";
            ButtonMinimise.Size = new Size(32, 32);
            ButtonMinimise.SizeMode = PictureBoxSizeMode.StretchImage;
            ButtonMinimise.TabIndex = 7;
            ButtonMinimise.TabStop = false;
            // 
            // ButtonClose
            // 
            ButtonClose.Image = Properties.Resources.icons8_close_window_32;
            ButtonClose.Location = new Point(155, 3);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(32, 32);
            ButtonClose.SizeMode = PictureBoxSizeMode.StretchImage;
            ButtonClose.TabIndex = 6;
            ButtonClose.TabStop = false;
            // 
            // MainPanel
            // 
            MainPanel.AutoScroll = true;
            MainPanel.Dock = DockStyle.Top;
            MainPanel.Location = new Point(0, 79);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(200, 274);
            MainPanel.TabIndex = 1;
            // 
            // MenuSplitter
            // 
            MenuSplitter.Dock = DockStyle.Top;
            MenuSplitter.Location = new Point(0, 353);
            MenuSplitter.Name = "MenuSplitter";
            MenuSplitter.Size = new Size(200, 4);
            MenuSplitter.TabIndex = 2;
            MenuSplitter.TabStop = false;
            // 
            // CommonPanel
            // 
            CommonPanel.AutoScroll = true;
            CommonPanel.Dock = DockStyle.Fill;
            CommonPanel.Location = new Point(0, 357);
            CommonPanel.Name = "CommonPanel";
            CommonPanel.Padding = new Padding(0, 0, 0, 10);
            CommonPanel.Size = new Size(200, 205);
            CommonPanel.TabIndex = 3;
            // 
            // MainMenuControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(CommonPanel);
            Controls.Add(MenuSplitter);
            Controls.Add(MainPanel);
            Controls.Add(NavPanel);
            Name = "MainMenuControl";
            Size = new Size(200, 562);
            NavPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ButtonRefresh).EndInit();
            ((System.ComponentModel.ISupportInitialize)ButtonPositionCubase).EndInit();
            ((System.ComponentModel.ISupportInitialize)ButtonBack).EndInit();
            ((System.ComponentModel.ISupportInitialize)ButtonMinimise).EndInit();
            ((System.ComponentModel.ISupportInitialize)ButtonClose).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel NavPanel;
        private Panel MainPanel;
        private Buttons.PictureButton ButtonClose;
        private Buttons.PictureButton ButtonMinimise;
        private Buttons.PictureButton ButtonBack;
        private Buttons.PictureButton ButtonPositionCubase;
        private Buttons.PictureButton ButtonRefresh;
        private Splitter MenuSplitter;
        private Panel CommonPanel;
        private Buttons.ActionButton ButtonReload;
        private Buttons.ActionButton ButtonCueLevels;
        private Buttons.ActionButton ButtonLyrics;
    }
}
