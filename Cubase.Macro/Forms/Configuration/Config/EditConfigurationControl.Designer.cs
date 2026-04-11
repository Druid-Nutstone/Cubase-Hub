namespace Cubase.Macro.Forms.Configuration.Config
{
    partial class EditConfigurationControl
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
            ResetVisibilityKey = new Cubase.Macro.BoundControls.BoundTextBox();
            SelectVisibilityKey = new Button();
            label2 = new Label();
            MenuHeight = new Cubase.Macro.BoundControls.BoundTextBox();
            label3 = new Label();
            KeyHeight = new Cubase.Macro.BoundControls.BoundTextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(24, 23);
            label1.Name = "label1";
            label1.Size = new Size(142, 20);
            label1.TabIndex = 0;
            label1.Text = "Reset Visibility Key";
            // 
            // ResetVisibilityKey
            // 
            ResetVisibilityKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ResetVisibilityKey.Location = new Point(24, 46);
            ResetVisibilityKey.Name = "ResetVisibilityKey";
            ResetVisibilityKey.Size = new Size(223, 27);
            ResetVisibilityKey.TabIndex = 1;
            // 
            // SelectVisibilityKey
            // 
            SelectVisibilityKey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SelectVisibilityKey.Location = new Point(253, 45);
            SelectVisibilityKey.Name = "SelectVisibilityKey";
            SelectVisibilityKey.Size = new Size(94, 29);
            SelectVisibilityKey.TabIndex = 2;
            SelectVisibilityKey.Text = "Select";
            SelectVisibilityKey.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(24, 94);
            label2.Name = "label2";
            label2.Size = new Size(153, 20);
            label2.TabIndex = 3;
            label2.Text = "Menu Button Height";
            // 
            // MenuHeight
            // 
            MenuHeight.Location = new Point(24, 117);
            MenuHeight.Name = "MenuHeight";
            MenuHeight.Size = new Size(125, 27);
            MenuHeight.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(24, 180);
            label3.Name = "label3";
            label3.Size = new Size(139, 20);
            label3.TabIndex = 5;
            label3.Text = "Key Button Height";
            // 
            // KeyHeight
            // 
            KeyHeight.Location = new Point(28, 205);
            KeyHeight.Name = "KeyHeight";
            KeyHeight.Size = new Size(125, 27);
            KeyHeight.TabIndex = 6;
            // 
            // EditConfigurationControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(KeyHeight);
            Controls.Add(label3);
            Controls.Add(MenuHeight);
            Controls.Add(label2);
            Controls.Add(SelectVisibilityKey);
            Controls.Add(ResetVisibilityKey);
            Controls.Add(label1);
            Name = "EditConfigurationControl";
            Size = new Size(366, 421);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private BoundControls.BoundTextBox ResetVisibilityKey;
        private Button SelectVisibilityKey;
        private Label label2;
        private BoundControls.BoundTextBox MenuHeight;
        private Label label3;
        private BoundControls.BoundTextBox KeyHeight;
    }
}
