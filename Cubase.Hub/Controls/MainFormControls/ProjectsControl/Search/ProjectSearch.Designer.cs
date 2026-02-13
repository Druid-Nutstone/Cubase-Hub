namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.Search
{
    partial class ProjectSearch
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
            label1 = new Label();
            FilterText = new TextBox();
            ClearFilter = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)ClearFilter).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(55, 20);
            label1.TabIndex = 0;
            label1.Text = "Search";
            // 
            // FilterText
            // 
            FilterText.Location = new Point(0, 23);
            FilterText.Name = "FilterText";
            FilterText.Size = new Size(230, 27);
            FilterText.TabIndex = 1;
            // 
            // ClearFilter
            // 
            ClearFilter.Cursor = Cursors.Hand;
            ClearFilter.Image = Properties.Resources.close;
            ClearFilter.Location = new Point(232, 23);
            ClearFilter.Name = "ClearFilter";
            ClearFilter.Size = new Size(29, 29);
            ClearFilter.SizeMode = PictureBoxSizeMode.StretchImage;
            ClearFilter.TabIndex = 2;
            ClearFilter.TabStop = false;
            // 
            // ProjectSearch
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ClearFilter);
            Controls.Add(FilterText);
            Controls.Add(label1);
            Name = "ProjectSearch";
            Size = new Size(281, 64);
            ((System.ComponentModel.ISupportInitialize)ClearFilter).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox FilterText;
        private PictureBox ClearFilter;
    }
}
