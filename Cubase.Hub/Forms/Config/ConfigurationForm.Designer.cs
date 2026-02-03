namespace Cubase.Hub.Forms.Config
{
    partial class ConfigurationForm
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
            ButtonSave = new Button();
            label1 = new Label();
            SourceCubaseFolders = new TextBox();
            AddSourceFolderButton = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(ButtonSave);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 398);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 52);
            panel1.TabIndex = 0;
            // 
            // ButtonSave
            // 
            ButtonSave.Location = new Point(12, 11);
            ButtonSave.Name = "ButtonSave";
            ButtonSave.Size = new Size(94, 29);
            ButtonSave.TabIndex = 0;
            ButtonSave.Text = "Save";
            ButtonSave.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(34, 26);
            label1.Name = "label1";
            label1.Size = new Size(165, 20);
            label1.TabIndex = 1;
            label1.Text = "Source Cubase Folders";
            // 
            // SourceCubaseFolders
            // 
            SourceCubaseFolders.Location = new Point(34, 49);
            SourceCubaseFolders.Name = "SourceCubaseFolders";
            SourceCubaseFolders.Size = new Size(638, 27);
            SourceCubaseFolders.TabIndex = 2;
            // 
            // AddSourceFolderButton
            // 
            AddSourceFolderButton.Location = new Point(678, 48);
            AddSourceFolderButton.Name = "AddSourceFolderButton";
            AddSourceFolderButton.Size = new Size(94, 29);
            AddSourceFolderButton.TabIndex = 3;
            AddSourceFolderButton.Text = "Add";
            AddSourceFolderButton.UseVisualStyleBackColor = true;
            // 
            // ConfigurationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(AddSourceFolderButton);
            Controls.Add(SourceCubaseFolders);
            Controls.Add(label1);
            Controls.Add(panel1);
            Name = "ConfigurationForm";
            Text = "ConfigurationForm";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button ButtonSave;
        private Label label1;
        private TextBox SourceCubaseFolders;
        private Button AddSourceFolderButton;
    }
}