using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Models.Lyrics;
using System.IO;

namespace Cubase.Macro.Forms.Lyrics.SetLists
{
    public partial class SetListManager : UserControl
    {
        private SetListContainer setList;

        public SetListManager()
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.EnsureSetListDirectory();
            this.setList = new SetListContainer();
            this.BindControls();
            this.SaveSetListButton.Click += SaveSetListButton_Click;
            this.NewSetListButton.Click += NewSetListButton_Click;
            this.SetListSelector.SelectedIndexChanged += SetListSelector_SelectedIndexChanged;
        }

        private void NewSetListButton_Click(object? sender, EventArgs e)
        {
            this.setList = new SetListContainer();
            this.BindControls();
        }

        private void SetListSelector_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var selectedSetlist = $"{this.SetListSelector.SelectedItem as string}{CubaseMacroConstants.NutstoneSetListNotation}";
            var selectedSetListPath = Path.Combine(CubaseMacroConstants.NutstoneSetListBaseDirectory, selectedSetlist);
            if (File.Exists(selectedSetListPath))
            {
                this.setList = SetListContainer.Load(selectedSetListPath);
                this.BindControls();
            }

        }

        private void SaveSetListButton_Click(object? sender, EventArgs e)
        {
            var targetFile = Path.Combine(CubaseMacroConstants.NutstoneSetListBaseDirectory, $"{this.setList.Title}{CubaseMacroConstants.NutstoneSetListNotation}");
            this.setList.Save(targetFile);
        }

        private void BindControls()
        {
            this.SetListName.Bind(nameof(SetListContainer.Title), this.setList);
            AllSongs.Bind(this.setList, this.LyricStateChanged);
            this.SetListLyricListView.Bind(this.setList.Songs, this.OnSelectedSongSelected);

            var setlistList = Directory.GetFiles(CubaseMacroConstants.NutstoneSetListBaseDirectory, $"*{CubaseMacroConstants.NutstoneSetListNotation}");
            this.SetListSelector.Items.Clear();
            this.SetListSelector.Items.AddRange(setlistList.Select(x => Path.GetFileNameWithoutExtension(x)).ToArray());
        }

        private void LyricStateChanged(SetListSong lyric, bool state)
        {
            if (state)
            {
                this.setList.Songs.Add(lyric);
            }
            else
            {
                var index = this.setList.Songs.FindIndex(x => x.Title == lyric.Title);
                if (index > -1)
                {
                    this.setList.Songs.RemoveAt(index);
                }
            }
            this.SetListLyricListView.Bind(this.setList.Songs, this.OnSelectedSongSelected);
        }

        private void OnSelectedSongSelected(SetListSong song)
        {
            this.setListLyricNotesEditor.BindSetListLyricNotes(song);
        }

        private void EnsureSetListDirectory()
        {
            if (!Directory.Exists(CubaseMacroConstants.NutstoneSetListBaseDirectory))
            {
                Directory.CreateDirectory(CubaseMacroConstants.NutstoneSetListBaseDirectory);
            }
        }
    }
}
