namespace Cubase.Macro.Forms.Lyrics
{
    partial class LyricViewerForm
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
            panel1 = new Panel();
            SaveButton = new Cubase.Macro.Forms.Main.Buttons.LyricButton();
            OpenButton = new Cubase.Macro.Forms.Main.Buttons.LyricButton();
            panel2 = new Panel();
            ScrollButton = new Cubase.Macro.Forms.Main.Buttons.LyricButton();
            TransPortLocation = new Label();
            FontDecrease = new Cubase.Macro.Forms.Main.Buttons.LyricButton();
            FontIncrease = new Cubase.Macro.Forms.Main.Buttons.LyricButton();
            EditButton = new Cubase.Macro.Forms.Main.Buttons.LyricButton();
            MainPanel = new Panel();
            MidiEnabled = new CheckBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(SaveButton);
            panel1.Controls.Add(OpenButton);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(FontDecrease);
            panel1.Controls.Add(FontIncrease);
            panel1.Controls.Add(EditButton);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 60);
            panel1.TabIndex = 0;
            // 
            // SaveButton
            // 
            SaveButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            SaveButton.Location = new Point(196, 15);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(61, 30);
            SaveButton.TabIndex = 6;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            // 
            // OpenButton
            // 
            OpenButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            OpenButton.Location = new Point(135, 15);
            OpenButton.Name = "OpenButton";
            OpenButton.Size = new Size(60, 30);
            OpenButton.TabIndex = 5;
            OpenButton.Text = "Open";
            OpenButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(MidiEnabled);
            panel2.Controls.Add(ScrollButton);
            panel2.Controls.Add(TransPortLocation);
            panel2.Dock = DockStyle.Right;
            panel2.Location = new Point(467, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(333, 60);
            panel2.TabIndex = 4;
            // 
            // ScrollButton
            // 
            ScrollButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ScrollButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ScrollButton.Location = new Point(145, 15);
            ScrollButton.Name = "ScrollButton";
            ScrollButton.Size = new Size(120, 30);
            ScrollButton.TabIndex = 1;
            ScrollButton.Text = "Start Scrolling";
            ScrollButton.UseVisualStyleBackColor = true;
            // 
            // TransPortLocation
            // 
            TransPortLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            TransPortLocation.AutoSize = true;
            TransPortLocation.Location = new Point(271, 20);
            TransPortLocation.Name = "TransPortLocation";
            TransPortLocation.Size = new Size(44, 20);
            TransPortLocation.TabIndex = 0;
            TransPortLocation.Text = "00:00";
            // 
            // FontDecrease
            // 
            FontDecrease.FlatAppearance.BorderSize = 0;
            FontDecrease.FlatStyle = FlatStyle.Flat;
            FontDecrease.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            FontDecrease.Location = new Point(67, 15);
            FontDecrease.Name = "FontDecrease";
            FontDecrease.Size = new Size(30, 30);
            FontDecrease.TabIndex = 3;
            FontDecrease.Text = "-";
            FontDecrease.UseVisualStyleBackColor = true;
            // 
            // FontIncrease
            // 
            FontIncrease.FlatAppearance.BorderSize = 0;
            FontIncrease.FlatStyle = FlatStyle.Flat;
            FontIncrease.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            FontIncrease.Location = new Point(36, 15);
            FontIncrease.Name = "FontIncrease";
            FontIncrease.Size = new Size(30, 30);
            FontIncrease.TabIndex = 2;
            FontIncrease.Text = "+";
            FontIncrease.UseVisualStyleBackColor = true;
            // 
            // EditButton
            // 
            EditButton.FlatAppearance.BorderSize = 0;
            EditButton.FlatStyle = FlatStyle.Flat;
            EditButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            EditButton.Location = new Point(5, 15);
            EditButton.Name = "EditButton";
            EditButton.Size = new Size(30, 30);
            EditButton.TabIndex = 0;
            EditButton.Text = "E";
            EditButton.UseVisualStyleBackColor = true;
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 60);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(800, 390);
            MainPanel.TabIndex = 1;
            // 
            // MidiEnabled
            // 
            MidiEnabled.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MidiEnabled.AutoSize = true;
            MidiEnabled.Location = new Point(38, 19);
            MidiEnabled.Name = "MidiEnabled";
            MidiEnabled.Size = new Size(89, 24);
            MidiEnabled.TabIndex = 2;
            MidiEnabled.Text = "Use Midi";
            MidiEnabled.UseVisualStyleBackColor = true;
            // 
            // LyricViewerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MainPanel);
            Controls.Add(panel1);
            Name = "LyricViewerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lyric Viewer";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel MainPanel;
        private Main.Buttons.LyricButton EditButton;
        private Main.Buttons.LyricButton FontDecrease;
        private Main.Buttons.LyricButton FontIncrease;
        private Panel panel2;
        private Label TransPortLocation;
        private Main.Buttons.LyricButton OpenButton;
        private Main.Buttons.LyricButton SaveButton;
        private Main.Buttons.LyricButton ScrollButton;
        private CheckBox MidiEnabled;
    }
}