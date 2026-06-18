namespace Cubase.Macro.Forms.Lyrics
{
    partial class LyricsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LyricsForm));
            TopMenu = new MenuStrip();
            ToolStrip = new ToolStrip();
            ViewButton = new ToolStripButton();
            EditButton = new ToolStripButton();
            AutoScroll = new ToolStripButton();
            MainPanel = new Panel();
            toolStripLabel1 = new ToolStripLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            TrackLocation = new ToolStripTextBox();
            ToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // TopMenu
            // 
            TopMenu.ImageScalingSize = new Size(20, 20);
            TopMenu.Location = new Point(0, 0);
            TopMenu.Name = "TopMenu";
            TopMenu.Size = new Size(800, 24);
            TopMenu.TabIndex = 0;
            TopMenu.Text = "menuStrip1";
            // 
            // ToolStrip
            // 
            ToolStrip.ImageScalingSize = new Size(20, 20);
            ToolStrip.Items.AddRange(new ToolStripItem[] { ViewButton, EditButton, AutoScroll, toolStripSeparator1, toolStripLabel1, TrackLocation });
            ToolStrip.Location = new Point(0, 24);
            ToolStrip.Name = "ToolStrip";
            ToolStrip.Size = new Size(800, 27);
            ToolStrip.TabIndex = 1;
            ToolStrip.Text = "toolStrip1";
            // 
            // ViewButton
            // 
            ViewButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ViewButton.Image = (Image)resources.GetObject("ViewButton.Image");
            ViewButton.ImageTransparentColor = Color.Magenta;
            ViewButton.Name = "ViewButton";
            ViewButton.Size = new Size(49, 24);
            ViewButton.Text = "View ";
            ViewButton.ToolTipText = "View Lyrics ";
            // 
            // EditButton
            // 
            EditButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            EditButton.Image = (Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new Size(39, 24);
            EditButton.Text = "Edit";
            // 
            // AutoScroll
            // 
            AutoScroll.DisplayStyle = ToolStripItemDisplayStyle.Text;
            AutoScroll.Image = (Image)resources.GetObject("AutoScroll.Image");
            AutoScroll.ImageTransparentColor = Color.Magenta;
            AutoScroll.Name = "AutoScroll";
            AutoScroll.Size = new Size(86, 24);
            AutoScroll.Text = "Auto Scroll";
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 51);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(800, 399);
            MainPanel.TabIndex = 2;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(104, 24);
            toolStripLabel1.Text = "Track Location";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 27);
            // 
            // TrackLocation
            // 
            TrackLocation.Name = "TrackLocation";
            TrackLocation.Size = new Size(100, 27);
            // 
            // LyricsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MainPanel);
            Controls.Add(ToolStrip);
            Controls.Add(TopMenu);
            MainMenuStrip = TopMenu;
            Name = "LyricsForm";
            RightToLeftLayout = true;
            Text = "Create and Amend Lyrics and Setlists";
            WindowState = FormWindowState.Maximized;
            ToolStrip.ResumeLayout(false);
            ToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip TopMenu;
        private ToolStrip ToolStrip;
        private Panel MainPanel;
        private ToolStripButton ViewButton;
        private ToolStripButton EditButton;
        private ToolStripButton AutoScroll;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripTextBox TrackLocation;
    }
}