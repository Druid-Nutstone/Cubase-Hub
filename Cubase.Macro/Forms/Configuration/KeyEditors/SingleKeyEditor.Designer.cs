namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    partial class SingleKeyEditor
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
            KeySelectorControl = new MacroKeySelectorControl();
            ReturnToParentMenuAfterExecution = new Cubase.Macro.BoundControls.BoundCheckbox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(15, 17);
            label1.Name = "label1";
            label1.Size = new Size(42, 20);
            label1.TabIndex = 0;
            label1.Text = "Keys";
            // 
            // KeySelectorControl
            // 
            KeySelectorControl.Location = new Point(15, 40);
            KeySelectorControl.Name = "KeySelectorControl";
            KeySelectorControl.Size = new Size(451, 157);
            KeySelectorControl.TabIndex = 1;
            // 
            // ReturnToParentMenuAfterExecution
            // 
            ReturnToParentMenuAfterExecution.AutoSize = true;
            ReturnToParentMenuAfterExecution.Location = new Point(15, 203);
            ReturnToParentMenuAfterExecution.Name = "ReturnToParentMenuAfterExecution";
            ReturnToParentMenuAfterExecution.Size = new Size(180, 24);
            ReturnToParentMenuAfterExecution.TabIndex = 2;
            ReturnToParentMenuAfterExecution.Text = "Return To Parent Menu";
            ReturnToParentMenuAfterExecution.UseVisualStyleBackColor = true;
            // 
            // SingleKeyEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ReturnToParentMenuAfterExecution);
            Controls.Add(KeySelectorControl);
            Controls.Add(label1);
            Name = "SingleKeyEditor";
            Size = new Size(527, 308);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private MacroKeySelectorControl KeySelectorControl;
        private Cubase.Macro.BoundControls.BoundCheckbox ReturnToParentMenuAfterExecution;
    }
}
