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
            label4 = new Label();
            CubaseExecutableName = new Cubase.Macro.BoundControls.BoundTextBox();
            CubaseProjectWindowStartsWith = new Cubase.Macro.BoundControls.BoundTextBox();
            label5 = new Label();
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
            MenuHeight.Size = new Size(142, 27);
            MenuHeight.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(208, 94);
            label3.Name = "label3";
            label3.Size = new Size(139, 20);
            label3.TabIndex = 5;
            label3.Text = "Key Button Height";
            // 
            // KeyHeight
            // 
            KeyHeight.Location = new Point(208, 117);
            KeyHeight.Name = "KeyHeight";
            KeyHeight.Size = new Size(139, 27);
            KeyHeight.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(24, 180);
            label4.Name = "label4";
            label4.Size = new Size(184, 20);
            label4.TabIndex = 7;
            label4.Text = "Cubase Executable Name";
            // 
            // CubaseExecutableName
            // 
            CubaseExecutableName.Location = new Point(24, 203);
            CubaseExecutableName.Name = "CubaseExecutableName";
            CubaseExecutableName.Size = new Size(316, 27);
            CubaseExecutableName.TabIndex = 8;
            // 
            // CubaseProjectWindowStartsWith
            // 
            CubaseProjectWindowStartsWith.Location = new Point(24, 271);
            CubaseProjectWindowStartsWith.Name = "CubaseProjectWindowStartsWith";
            CubaseProjectWindowStartsWith.Size = new Size(316, 27);
            CubaseProjectWindowStartsWith.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(24, 248);
            label5.Name = "label5";
            label5.Size = new Size(257, 20);
            label5.TabIndex = 9;
            label5.Text = "Cubase Project Window Starts With";
            // 
            // EditConfigurationControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(CubaseProjectWindowStartsWith);
            Controls.Add(label5);
            Controls.Add(CubaseExecutableName);
            Controls.Add(label4);
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
        private Label label4;
        private BoundControls.BoundTextBox CubaseExecutableName;
        private BoundControls.BoundTextBox CubaseProjectWindowStartsWith;
        private Label label5;
    }
}
