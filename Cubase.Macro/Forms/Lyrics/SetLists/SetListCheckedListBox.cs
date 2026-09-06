using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Models.Lyrics;
using System.IO;

namespace Cubase.Macro.Forms.Lyrics.SetLists
{
    public class SetListCheckedListBox : CheckedListBox
    {
        private bool initialising = false;

        private Action<SetListSong, bool> OnLyricStateChanged;

        private string[] allLyrics;

        private SetListContainer setlist;



        public SetListCheckedListBox() : base()
        {
            this.CheckOnClick = true;
        }

        public void Bind(SetListContainer setList, Action<SetListSong, bool> onLyricStateChanged)
        {
            initialising = true;
            this.setlist = setList;
            this.OnLyricStateChanged = onLyricStateChanged;
            this.Items.Clear();
            this.allLyrics = Directory.GetFiles(CubaseMacroConstants.NutstoneLyricBaseDirectory, $"*{CubaseMacroConstants.NutstoneLyricNotation}");
            var allSongs = this.allLyrics.Select(x => Path.GetFileNameWithoutExtension(x));
            this.Items.AddRange(allSongs.ToArray());

            for (var i = 0; i < this.Items.Count; i++)
            {
                var matchedTitle = setList.Songs.FirstOrDefault(x => x.Title == this.Items[i].ToString());
                if (matchedTitle != null)
                {
                    this.SetItemChecked(i, true);
                }
            }
            initialising = false;
        }

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            base.OnItemCheck(ice);
            if (!initialising)
            {
                var setListSong = new SetListSong()
                {
                    FileName = Path.GetFileName(this.allLyrics[ice.Index]),
                    Title = this.Items[ice.Index].ToString(),
                    Index = this.setlist.Songs.Count
                };

                this.OnLyricStateChanged?.Invoke(setListSong, ice.NewValue == CheckState.Checked);
            }
        }

    }
}
