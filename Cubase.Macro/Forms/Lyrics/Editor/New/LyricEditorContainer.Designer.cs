namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    partial class LyricEditorContainer
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
            EditorTopPanel = new Panel();
            label6 = new Label();
            AddSection = new Cubase.Macro.Forms.Main.Buttons.ActionButton();
            TimeSignature = new Cubase.Hub.Controls.BoundControls.BoundNumericTextBox();
            label5 = new Label();
            Duration = new DateTimePicker();
            label4 = new Label();
            BPM = new Cubase.Hub.Controls.BoundControls.BoundNumericTextBox();
            label3 = new Label();
            Album = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            label2 = new Label();
            SongTitle = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            label1 = new Label();
            SectionPanel = new Panel();
            FontSize = new Cubase.Hub.Controls.BoundControls.BoundNumericTextBox();
            EditorTopPanel.SuspendLayout();
            SuspendLayout();
            // 
            // EditorTopPanel
            // 
            EditorTopPanel.Controls.Add(FontSize);
            EditorTopPanel.Controls.Add(label6);
            EditorTopPanel.Controls.Add(AddSection);
            EditorTopPanel.Controls.Add(TimeSignature);
            EditorTopPanel.Controls.Add(label5);
            EditorTopPanel.Controls.Add(Duration);
            EditorTopPanel.Controls.Add(label4);
            EditorTopPanel.Controls.Add(BPM);
            EditorTopPanel.Controls.Add(label3);
            EditorTopPanel.Controls.Add(Album);
            EditorTopPanel.Controls.Add(label2);
            EditorTopPanel.Controls.Add(SongTitle);
            EditorTopPanel.Controls.Add(label1);
            EditorTopPanel.Dock = DockStyle.Top;
            EditorTopPanel.Location = new Point(0, 0);
            EditorTopPanel.Name = "EditorTopPanel";
            EditorTopPanel.Size = new Size(750, 149);
            EditorTopPanel.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(434, 88);
            label6.Name = "label6";
            label6.Size = new Size(72, 20);
            label6.TabIndex = 11;
            label6.Text = "Font Size";
            // 
            // AddSection
            // 
            AddSection.FlatAppearance.BorderSize = 0;
            AddSection.FlatStyle = FlatStyle.Flat;
            AddSection.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            AddSection.Location = new Point(617, 98);
            AddSection.Name = "AddSection";
            AddSection.Size = new Size(111, 38);
            AddSection.TabIndex = 10;
            AddSection.Text = "Add Section";
            AddSection.UseVisualStyleBackColor = true;
            // 
            // TimeSignature
            // 
            TimeSignature.Location = new Point(270, 111);
            TimeSignature.Name = "TimeSignature";
            TimeSignature.Size = new Size(94, 27);
            TimeSignature.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(270, 86);
            label5.Name = "label5";
            label5.Size = new Size(115, 20);
            label5.TabIndex = 8;
            label5.Text = "Time Signature";
            // 
            // Duration
            // 
            Duration.Format = DateTimePickerFormat.Time;
            Duration.Location = new Point(139, 112);
            Duration.Name = "Duration";
            Duration.Size = new Size(115, 27);
            Duration.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(139, 86);
            label4.Name = "label4";
            label4.Size = new Size(71, 20);
            label4.TabIndex = 6;
            label4.Text = "Duration";
            // 
            // BPM
            // 
            BPM.Location = new Point(26, 111);
            BPM.Name = "BPM";
            BPM.Size = new Size(94, 27);
            BPM.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(26, 88);
            label3.Name = "label3";
            label3.Size = new Size(42, 20);
            label3.TabIndex = 4;
            label3.Text = "BPM";
            // 
            // Album
            // 
            Album.Location = new Point(434, 41);
            Album.Name = "Album";
            Album.Size = new Size(294, 27);
            Album.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(434, 18);
            label2.Name = "label2";
            label2.Size = new Size(56, 20);
            label2.TabIndex = 2;
            label2.Text = "Album";
            // 
            // SongTitle
            // 
            SongTitle.Location = new Point(26, 41);
            SongTitle.Name = "SongTitle";
            SongTitle.Size = new Size(359, 27);
            SongTitle.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(26, 18);
            label1.Name = "label1";
            label1.Size = new Size(79, 20);
            label1.TabIndex = 0;
            label1.Text = "Song Title";
            // 
            // SectionPanel
            // 
            SectionPanel.AutoScroll = true;
            SectionPanel.Dock = DockStyle.Fill;
            SectionPanel.Location = new Point(0, 149);
            SectionPanel.Name = "SectionPanel";
            SectionPanel.Size = new Size(750, 223);
            SectionPanel.TabIndex = 1;
            // 
            // FontSize
            // 
            FontSize.Location = new Point(434, 109);
            FontSize.Name = "FontSize";
            FontSize.Size = new Size(94, 27);
            FontSize.TabIndex = 12;
            // 
            // LyricEditorContainer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(SectionPanel);
            Controls.Add(EditorTopPanel);
            Name = "LyricEditorContainer";
            Size = new Size(750, 372);
            EditorTopPanel.ResumeLayout(false);
            EditorTopPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel EditorTopPanel;
        private Panel SectionPanel;
        private Label label1;
        private Hub.Controls.BoundControls.BoundTextBox SongTitle;
        private Hub.Controls.BoundControls.BoundTextBox Album;
        private Label label2;
        private Hub.Controls.BoundControls.BoundNumericTextBox BPM;
        private Label label3;
        private Label label4;
        private Hub.Controls.BoundControls.BoundNumericTextBox TimeSignature;
        private Label label5;
        private DateTimePicker Duration;
        private Main.Buttons.ActionButton AddSection;
        private Label label6;
        private Hub.Controls.BoundControls.BoundNumericTextBox FontSize;
    }
}
