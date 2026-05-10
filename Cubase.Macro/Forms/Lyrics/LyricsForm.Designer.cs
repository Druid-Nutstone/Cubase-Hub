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
            TopMenu = new MenuStrip();
            MainPanel = new Panel();
            lyricEditor = new Cubase.Macro.Forms.Lyrics.Editor.LyricEditor();
            MainPanel.SuspendLayout();
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
            // MainPanel
            // 
            MainPanel.Controls.Add(lyricEditor);
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 24);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(800, 426);
            MainPanel.TabIndex = 1;
            // 
            // lyricEditor
            // 
            lyricEditor.BorderStyle = BorderStyle.None;
            lyricEditor.Dock = DockStyle.Fill;
            lyricEditor.Font = new Font("Segoe UI", 12F);
            lyricEditor.ForeColor = Color.White;
            lyricEditor.Location = new Point(0, 0);
            lyricEditor.Name = "lyricEditor";
            lyricEditor.Size = new Size(800, 426);
            lyricEditor.TabIndex = 0;
            lyricEditor.Text = "";
            // 
            // LyricsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MainPanel);
            Controls.Add(TopMenu);
            MainMenuStrip = TopMenu;
            Name = "LyricsForm";
            RightToLeftLayout = true;
            Text = "Create and Amend Lyrics and Setlists";
            WindowState = FormWindowState.Maximized;
            MainPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip TopMenu;
        private Panel MainPanel;
        private Editor.LyricEditor lyricEditor;
    }
}