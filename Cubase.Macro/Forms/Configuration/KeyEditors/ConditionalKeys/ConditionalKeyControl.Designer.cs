namespace Cubase.Macro.Forms.Configuration.KeyEditors.ConditionalKeys
{
    partial class ConditionalKeyControl
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
            Function = new ComboBox();
            Property = new ComboBox();
            Condition = new ComboBox();
            SuspendLayout();
            // 
            // Function
            // 
            Function.FormattingEnabled = true;
            Function.Location = new Point(17, 10);
            Function.Name = "Function";
            Function.Size = new Size(151, 28);
            Function.TabIndex = 0;
            // 
            // Property
            // 
            Property.FormattingEnabled = true;
            Property.Location = new Point(174, 10);
            Property.Name = "Property";
            Property.Size = new Size(187, 28);
            Property.TabIndex = 1;
            // 
            // Condition
            // 
            Condition.FormattingEnabled = true;
            Condition.Location = new Point(367, 10);
            Condition.Name = "Condition";
            Condition.Size = new Size(151, 28);
            Condition.TabIndex = 2;
            // 
            // ConditionalKeyControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Condition);
            Controls.Add(Property);
            Controls.Add(Function);
            Name = "ConditionalKeyControl";
            Size = new Size(534, 47);
            ResumeLayout(false);
        }

        #endregion

        private ComboBox Function;
        private ComboBox Property;
        private ComboBox Condition;
    }
}
