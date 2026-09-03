namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    partial class SectionControl
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
            panel1 = new Panel();
            SectionComments = new Cubase.Macro.BoundControls.BoundTextBox();
            label3 = new Label();
            SectionBar = new Cubase.Macro.BoundControls.BoundNumericTextBox();
            label2 = new Label();
            SectionName = new Cubase.Macro.BoundControls.BoundTextBox();
            Label1 = new Label();
            LyricPanel = new Panel();
            label4 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label4);
            panel1.Controls.Add(SectionComments);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(SectionBar);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(SectionName);
            panel1.Controls.Add(Label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(456, 91);
            panel1.TabIndex = 0;
            // 
            // SectionComments
            // 
            SectionComments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SectionComments.Location = new Point(274, 34);
            SectionComments.Name = "SectionComments";
            SectionComments.Size = new Size(167, 27);
            SectionComments.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(274, 11);
            label3.Name = "label3";
            label3.Size = new Size(85, 20);
            label3.TabIndex = 4;
            label3.Text = "Comments";
            // 
            // SectionBar
            // 
            SectionBar.Location = new Point(174, 34);
            SectionBar.Name = "SectionBar";
            SectionBar.Size = new Size(73, 27);
            SectionBar.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(174, 11);
            label2.Name = "label2";
            label2.Size = new Size(33, 20);
            label2.TabIndex = 2;
            label2.Text = "Bar";
            // 
            // SectionName
            // 
            SectionName.Location = new Point(19, 34);
            SectionName.Name = "SectionName";
            SectionName.Size = new Size(131, 27);
            SectionName.TabIndex = 1;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Label1.Location = new Point(19, 11);
            Label1.Name = "Label1";
            Label1.Size = new Size(106, 20);
            Label1.TabIndex = 0;
            Label1.Text = "Section Name";
            // 
            // LyricPanel
            // 
            LyricPanel.Dock = DockStyle.Fill;
            LyricPanel.Location = new Point(0, 91);
            LyricPanel.Name = "LyricPanel";
            LyricPanel.Size = new Size(456, 169);
            LyricPanel.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.ScrollBar;
            label4.Location = new Point(0, 68);
            label4.Name = "label4";
            label4.Size = new Size(44, 20);
            label4.TabIndex = 6;
            label4.Text = "Lyrics";
            // 
            // SectionControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(LyricPanel);
            Controls.Add(panel1);
            Name = "SectionControl";
            Size = new Size(456, 260);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label Label1;
        private BoundControls.BoundTextBox SectionName;
        private BoundControls.BoundNumericTextBox SectionBar;
        private Label label2;
        private BoundControls.BoundTextBox SectionComments;
        private Label label3;
        private Panel LyricPanel;
        private Label label4;
    }
}
