namespace Cubase.Macro.Forms.Configuration.Config
{
    public class CubaseSourceLocationListView : ListView
    {
        public CubaseSourceLocationListView() : base()
        {
            this.View = View.Details;
            this.MultiSelect = false;
            this.DoubleBuffered = true;
            this.AddHeader("Location");
        }

        private void AddHeader(string text)
        {
            this.Columns.Add(text);
        }

        public void AddSourceDirectory(string directory)
        {
            this.Items.Add(new CubaseSourceLocationListViewItem(directory));
            this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }

    public class CubaseSourceLocationListViewItem : ListViewItem
    {
        public CubaseSourceLocationListViewItem(string sourceDirectory)
        {
            this.Text = sourceDirectory;
        }
    }
}
