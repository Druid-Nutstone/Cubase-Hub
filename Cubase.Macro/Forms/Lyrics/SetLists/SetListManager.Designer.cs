namespace Cubase.Macro.Forms.Lyrics.SetLists
{
    partial class SetListManager
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
            label2 = new Label();
            SetListName = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            label3 = new Label();
            AllSongs = new SetListCheckedListBox();
            SaveSetListButton = new Button();
            label4 = new Label();
            SetListLyricListView = new SetListLyricListView();
            SetListSelector = new ComboBox();
            NewSetListButton = new Button();
            setListLyricNotesEditor = new SetListNotesEditor();
            label5 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(35, 23);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 1;
            label1.Text = "Select Set list";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(228, 23);
            label2.Name = "label2";
            label2.Size = new Size(96, 20);
            label2.TabIndex = 2;
            label2.Text = "New Set List";
            // 
            // SetListName
            // 
            SetListName.Location = new Point(228, 46);
            SetListName.Name = "SetListName";
            SetListName.Size = new Size(185, 27);
            SetListName.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(35, 114);
            label3.Name = "label3";
            label3.Size = new Size(48, 20);
            label3.TabIndex = 4;
            label3.Text = "Lyrics";
            // 
            // AllSongs
            // 
            AllSongs.CheckOnClick = true;
            AllSongs.FormattingEnabled = true;
            AllSongs.Location = new Point(36, 137);
            AllSongs.Name = "AllSongs";
            AllSongs.Size = new Size(150, 290);
            AllSongs.TabIndex = 5;
            // 
            // SaveSetListButton
            // 
            SaveSetListButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SaveSetListButton.Location = new Point(441, 45);
            SaveSetListButton.Name = "SaveSetListButton";
            SaveSetListButton.Size = new Size(115, 29);
            SaveSetListButton.TabIndex = 6;
            SaveSetListButton.Text = "Save Set list";
            SaveSetListButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(228, 114);
            label4.Name = "label4";
            label4.Size = new Size(103, 20);
            label4.TabIndex = 7;
            label4.Text = "Set List Lyrics";
            // 
            // SetListLyricListView
            // 
            SetListLyricListView.AllowDrop = true;
            SetListLyricListView.FullRowSelect = true;
            SetListLyricListView.Location = new Point(228, 137);
            SetListLyricListView.Name = "SetListLyricListView";
            SetListLyricListView.Size = new Size(151, 290);
            SetListLyricListView.TabIndex = 8;
            SetListLyricListView.UseCompatibleStateImageBehavior = false;
            SetListLyricListView.View = View.Details;
            // 
            // SetListSelector
            // 
            SetListSelector.FormattingEnabled = true;
            SetListSelector.Location = new Point(35, 46);
            SetListSelector.Name = "SetListSelector";
            SetListSelector.Size = new Size(151, 28);
            SetListSelector.TabIndex = 9;
            // 
            // NewSetListButton
            // 
            NewSetListButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            NewSetListButton.Location = new Point(574, 44);
            NewSetListButton.Name = "NewSetListButton";
            NewSetListButton.Size = new Size(117, 29);
            NewSetListButton.TabIndex = 10;
            NewSetListButton.Text = "New Set List";
            NewSetListButton.UseVisualStyleBackColor = true;
            // 
            // setListLyricNotesEditor
            // 
            setListLyricNotesEditor.BorderStyle = BorderStyle.FixedSingle;
            setListLyricNotesEditor.Location = new Point(408, 137);
            setListLyricNotesEditor.Name = "setListLyricNotesEditor";
            setListLyricNotesEditor.Size = new Size(283, 140);
            setListLyricNotesEditor.TabIndex = 11;
            setListLyricNotesEditor.Text = "";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(408, 114);
            label5.Name = "label5";
            label5.Size = new Size(142, 20);
            label5.TabIndex = 12;
            label5.Text = "Set List Lyric Notes";
            // 
            // SetListManager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label5);
            Controls.Add(setListLyricNotesEditor);
            Controls.Add(NewSetListButton);
            Controls.Add(SetListSelector);
            Controls.Add(SetListLyricListView);
            Controls.Add(label4);
            Controls.Add(SaveSetListButton);
            Controls.Add(AllSongs);
            Controls.Add(label3);
            Controls.Add(SetListName);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SetListManager";
            Size = new Size(707, 460);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private Hub.Controls.BoundControls.BoundTextBox SetListName;
        private Label label3;
        private SetListCheckedListBox AllSongs;
        private Button SaveSetListButton;
        private Label label4;
        private SetListLyricListView SetListLyricListView;
        private ComboBox SetListSelector;
        private Button NewSetListButton;
        private SetListNotesEditor setListLyricNotesEditor;
        private Label label5;
    }
}
