namespace Cubase.Macro.Forms.Configuration
{
    partial class EditMacroControl
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
            UpdateButton = new Button();
            label1 = new Label();
            MacroTitle = new Cubase.Macro.BoundControls.BoundTextBox();
            panel1 = new Panel();
            panel2 = new Panel();
            label3 = new Label();
            MacroMenuChangesVisibility = new Cubase.Macro.BoundControls.BoundCheckbox();
            label2 = new Label();
            ExampleToggled = new Button();
            ExampleSingle = new Button();
            ToggleForgroundColour = new Cubase.Macro.Forms.Configuration.ColourPicker.ColourPickerControl();
            ToggleBackgroundColour = new Cubase.Macro.Forms.Configuration.ColourPicker.ColourPickerControl();
            ForegroundColour = new Cubase.Macro.Forms.Configuration.ColourPicker.ColourPickerControl();
            BackgroundColour = new Cubase.Macro.Forms.Configuration.ColourPicker.ColourPickerControl();
            MacroTitleToggled = new Cubase.Macro.BoundControls.BoundTextBox();
            label5 = new Label();
            MacroButtonType = new Cubase.Macro.BoundControls.BoundEnumComboBox();
            label4 = new Label();
            ContentPanel = new Panel();
            MacroAvailableForMacro = new Cubase.Macro.BoundControls.BoundCheckbox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // UpdateButton
            // 
            UpdateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            UpdateButton.Location = new Point(826, 12);
            UpdateButton.Name = "UpdateButton";
            UpdateButton.Size = new Size(93, 29);
            UpdateButton.TabIndex = 0;
            UpdateButton.Text = "Update";
            UpdateButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(23, 15);
            label1.Name = "label1";
            label1.Size = new Size(40, 20);
            label1.TabIndex = 1;
            label1.Text = "Title";
            // 
            // MacroTitle
            // 
            MacroTitle.Location = new Point(23, 38);
            MacroTitle.Name = "MacroTitle";
            MacroTitle.Size = new Size(173, 27);
            MacroTitle.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(UpdateButton);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 436);
            panel1.Name = "panel1";
            panel1.Size = new Size(934, 55);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(MacroAvailableForMacro);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(MacroMenuChangesVisibility);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(ExampleToggled);
            panel2.Controls.Add(ExampleSingle);
            panel2.Controls.Add(ToggleForgroundColour);
            panel2.Controls.Add(ToggleBackgroundColour);
            panel2.Controls.Add(ForegroundColour);
            panel2.Controls.Add(BackgroundColour);
            panel2.Controls.Add(MacroTitleToggled);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(MacroButtonType);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(MacroTitle);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(934, 262);
            panel2.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(590, 15);
            label3.Name = "label3";
            label3.Size = new Size(152, 20);
            label3.TabIndex = 17;
            label3.Text = "Available For Mobile";
            // 
            // MacroMenuChangesVisibility
            // 
            MacroMenuChangesVisibility.AutoSize = true;
            MacroMenuChangesVisibility.Location = new Point(432, 40);
            MacroMenuChangesVisibility.Name = "MacroMenuChangesVisibility";
            MacroMenuChangesVisibility.Size = new Size(78, 24);
            MacroMenuChangesVisibility.TabIndex = 16;
            MacroMenuChangesVisibility.Text = "Yes/No";
            MacroMenuChangesVisibility.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(432, 15);
            label2.Name = "label2";
            label2.Size = new Size(136, 20);
            label2.TabIndex = 15;
            label2.Text = " Changes Visibility";
            // 
            // ExampleToggled
            // 
            ExampleToggled.Location = new Point(485, 173);
            ExampleToggled.Name = "ExampleToggled";
            ExampleToggled.Size = new Size(157, 51);
            ExampleToggled.TabIndex = 14;
            ExampleToggled.Text = "Toggled";
            ExampleToggled.UseVisualStyleBackColor = true;
            // 
            // ExampleSingle
            // 
            ExampleSingle.Location = new Point(39, 173);
            ExampleSingle.Name = "ExampleSingle";
            ExampleSingle.Size = new Size(157, 51);
            ExampleSingle.TabIndex = 13;
            ExampleSingle.Text = "Not Toggled";
            ExampleSingle.UseVisualStyleBackColor = true;
            // 
            // ToggleForgroundColour
            // 
            ToggleForgroundColour.Location = new Point(708, 88);
            ToggleForgroundColour.Name = "ToggleForgroundColour";
            ToggleForgroundColour.Size = new Size(211, 68);
            ToggleForgroundColour.TabIndex = 12;
            // 
            // ToggleBackgroundColour
            // 
            ToggleBackgroundColour.Location = new Point(473, 88);
            ToggleBackgroundColour.Name = "ToggleBackgroundColour";
            ToggleBackgroundColour.Size = new Size(211, 68);
            ToggleBackgroundColour.TabIndex = 11;
            // 
            // ForegroundColour
            // 
            ForegroundColour.Location = new Point(256, 88);
            ForegroundColour.Name = "ForegroundColour";
            ForegroundColour.Size = new Size(211, 68);
            ForegroundColour.TabIndex = 10;
            // 
            // BackgroundColour
            // 
            BackgroundColour.Location = new Point(23, 88);
            BackgroundColour.Name = "BackgroundColour";
            BackgroundColour.Size = new Size(211, 68);
            BackgroundColour.TabIndex = 9;
            // 
            // MacroTitleToggled
            // 
            MacroTitleToggled.Location = new Point(230, 38);
            MacroTitleToggled.Name = "MacroTitleToggled";
            MacroTitleToggled.Size = new Size(168, 27);
            MacroTitleToggled.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(230, 15);
            label5.Name = "label5";
            label5.Size = new Size(100, 20);
            label5.TabIndex = 7;
            label5.Text = "Title Toggled";
            // 
            // MacroButtonType
            // 
            MacroButtonType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MacroButtonType.FormattingEnabled = true;
            MacroButtonType.Location = new Point(768, 38);
            MacroButtonType.Name = "MacroButtonType";
            MacroButtonType.Size = new Size(151, 28);
            MacroButtonType.TabIndex = 6;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(768, 15);
            label4.Name = "label4";
            label4.Size = new Size(95, 20);
            label4.TabIndex = 5;
            label4.Text = "Button Type";
            // 
            // ContentPanel
            // 
            ContentPanel.Dock = DockStyle.Fill;
            ContentPanel.Location = new Point(0, 262);
            ContentPanel.Name = "ContentPanel";
            ContentPanel.Size = new Size(934, 174);
            ContentPanel.TabIndex = 5;
            // 
            // MacroAvailableForMacro
            // 
            MacroAvailableForMacro.AutoSize = true;
            MacroAvailableForMacro.Location = new Point(590, 42);
            MacroAvailableForMacro.Name = "MacroAvailableForMacro";
            MacroAvailableForMacro.Size = new Size(78, 24);
            MacroAvailableForMacro.TabIndex = 18;
            MacroAvailableForMacro.Text = "Yes/No";
            MacroAvailableForMacro.UseVisualStyleBackColor = true;
            // 
            // EditMacroControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ContentPanel);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "EditMacroControl";
            Size = new Size(934, 491);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button UpdateButton;
        private Label label1;
        private BoundControls.BoundTextBox MacroTitle;
        private Panel panel1;
        private Panel panel2;
        private Panel ContentPanel;
        private BoundControls.BoundEnumComboBox MacroButtonType;
        private Label label4;
        private BoundControls.BoundTextBox MacroTitleToggled;
        private Label label5;
        private ColourPicker.ColourPickerControl BackgroundColour;
        private ColourPicker.ColourPickerControl ForegroundColour;
        private ColourPicker.ColourPickerControl ToggleForgroundColour;
        private ColourPicker.ColourPickerControl ToggleBackgroundColour;
        private Button ExampleSingle;
        private Button ExampleToggled;
        private BoundControls.BoundCheckbox MacroMenuChangesVisibility;
        private Label label2;
        private Label label3;
        private BoundControls.BoundCheckbox MacroAvailableForMacro;
    }
}
