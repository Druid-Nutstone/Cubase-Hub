using Cubase.Macro.Common.Lyrics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    public class LyricCompletetionListBox : TextBox
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<LyricControlCommand?> OnSelected { get; set; }

        private List<LyricControlCommand> commandList;

        public LyricCompletetionListBox() : base()
        {
            this.AutoCompleteMode = AutoCompleteMode.Suggest;
            //this.DrawMode = DrawMode.OwnerDrawFixed;
            //this.ItemHeight = 40;
            this.BorderStyle = BorderStyle.None;
            ThemeApplier.ApplyDarkTheme(this);
        }



        public void Populate(List<LyricControlCommand> list)
        {
            this.commandList = list;
            this.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            this.AutoCompleteCustomSource.AddRange(list.Select(x => $"{x.Description}{'\a'} {x.Keyword.ToString()} {x.Text}").ToArray());
            this.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                this.OnSelected?.Invoke(GetSelected());
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.OnSelected?.Invoke(null);
                return;
            }
        }


        public LyricControlCommand GetSelected()
        {
            var cntrlbits = this.Text.Split('\a');
            var extra = cntrlbits[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var keyWord = (ControlLyricKeyword)Enum.Parse(typeof(ControlLyricKeyword), extra[0]);
            return this.commandList.FirstOrDefault(x => x.Keyword == keyWord);
        }

    }
}
