using Cubase.Macro.Common.Models.Lyrics;

namespace Cubase.Macro.Forms.Lyrics.SetLists
{
    public class SetListLyricListView : ListView
    {
        private List<SetListSong> lyrics;

        private Action<SetListSong> OnSetListSongSelected;

        public SetListLyricListView() : base()
        {
            this.View = View.Details;
            this.DoubleBuffered = true;
            this.AllowDrop = true;
            this.FullRowSelect = true;
            this.MultiSelect = false;
            this.AddHeader("Lyric");
        }

        public void AddHeader(string name)
        {
            this.Columns.Add(name, -2, HorizontalAlignment.Left);
        }

        public void Bind(List<SetListSong> lyrics, Action<SetListSong> onSetListSongSelected)
        {
            this.lyrics = lyrics;
            this.OnSetListSongSelected = onSetListSongSelected;
            this.Items.Clear();
            foreach (var song in lyrics.OrderBy(x => x.Index))
            {
                this.Items.Add(new SetListLyricListViewItem(song));
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            var selected = this.SelectedItems.Count;
            if (selected > 0)
            {
                var song = this.SelectedItems[0];
                var songAsLyric = (SetListLyricListViewItem)song;
                this.OnSetListSongSelected?.Invoke(songAsLyric?.Song);
            }
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            drgevent.Effect = DragDropEffects.Move;
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);

            Point targetPoint = this.PointToClient(new Point(drgevent.X, drgevent.Y));
            int targetIndex = this.InsertionMark.NearestIndex(targetPoint);

            if (targetIndex > -1)
            {
                Rectangle itemBounds = this.GetItemRect(targetIndex);
                if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    this.InsertionMark.Index = targetIndex;
                    this.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    this.InsertionMark.Index = targetIndex;
                    this.InsertionMark.AppearsAfterItem = false;
                }
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
            this.InsertionMark.Index = -1;
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            this.InsertionMark.Index = -1;

            if (drgevent.Data.GetDataPresent(typeof(SetListLyricListViewItem)))
            {
                Point targetPoint = this.PointToClient(new Point(drgevent.X, drgevent.Y));
                int targetIndex = this.InsertionMark.NearestIndex(targetPoint);

                if (targetIndex == -1)
                {
                    targetIndex = this.Items.Count - 1;
                }

                if (this.InsertionMark.AppearsAfterItem)
                {
                    targetIndex++;
                }

                SetListLyricListViewItem draggedItem = (SetListLyricListViewItem)drgevent.Data.GetData(typeof(SetListLyricListViewItem));

                int oldIndex = draggedItem.Index;
                if (oldIndex < targetIndex)
                {
                    targetIndex--;
                }

                if (oldIndex == targetIndex) return;

                this.Items.Remove(draggedItem);
                this.Items.Insert(targetIndex, draggedItem);

                UpdateSongIndices();
            }
        }

        private void UpdateSongIndices()
        {
            if (lyrics == null) return;

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i] is SetListLyricListViewItem listViewItem)
                {
                    listViewItem.Song.Index = i;
                }
            }

            lyrics = this.Items.Cast<SetListLyricListViewItem>()
                               .Select(item => item.Song)
                               .ToList();
        }
    }

    public class SetListLyricListViewItem : ListViewItem
    {
        public SetListSong Song { get; set; }

        public SetListLyricListViewItem(SetListSong song)
        {
            this.Song = song;
            this.Text = song.Title;
            this.ForeColor = Color.White;
        }
    }
}