namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    partial class ProjectsControl
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
            IndexPanel = new Panel();
            toolStrip = new ToolStrip();
            DataPanel = new DarkScrollPanel();
            SuspendLayout();
            // 
            // IndexPanel
            // 
            IndexPanel.Dock = DockStyle.Left;
            IndexPanel.Location = new Point(0, 0);
            IndexPanel.Name = "IndexPanel";
            IndexPanel.Size = new Size(125, 445);
            IndexPanel.TabIndex = 0;
            // 
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ActiveCaptionText;
            toolStrip.ImageScalingSize = new Size(20, 20);
            toolStrip.Location = new Point(125, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(502, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolSrtip";
            // 
            // DataPanel
            // 
            DataPanel.AutoScroll = true;
            DataPanel.BackColor = Color.FromArgb(37, 37, 38);
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(125, 25);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(502, 420);
            DataPanel.TabIndex = 2;
            // 
            // ProjectsControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DataPanel);
            Controls.Add(toolStrip);
            Controls.Add(IndexPanel);
            Name = "ProjectsControl";
            Size = new Size(627, 445);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel IndexPanel;
        private ToolStrip toolStrip;
        private DarkScrollPanel DataPanel;
    }
}
