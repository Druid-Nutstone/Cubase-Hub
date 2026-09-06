using Cubase.Macro.Common.Models.Lyrics;

namespace Cubase.Macro.Forms.Lyrics.SetLists
{
    public class SetListNotesEditor : RichTextBox
    {

        private SetListSong setListSongNotes;

        private SetListContainer setListNotes;


        public SetListNotesEditor() : base()
        {
            this.BorderStyle = BorderStyle.None;
        }

        public void BindSetListNotes(SetListContainer setListNotes)
        {
            this.setListNotes = setListNotes;
            this.Text = setListNotes.Notes;
        }

        public void BindSetListLyricNotes(SetListSong setListSongNotes)
        {
            this.setListSongNotes = setListSongNotes;
            this.Text = setListSongNotes.Notes;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (setListSongNotes != null)
            {
                setListSongNotes?.Notes = this.Text;
            }
            else
            {
                setListNotes?.Notes = this.Text;
            }
        }
    }
}
