namespace Cubase.Macro.Forms.Scripts.Editor
{
    public class EditScriptMenu
    {
        private readonly IServiceProvider serviceProvider;

        public EditScriptMenu(MenuStrip parentMenu, IServiceProvider serviceProvider, EditScriptForm parent)
        {
            this.serviceProvider = serviceProvider;

            parentMenu.Items.Add(new FileMenu(parentMenu, serviceProvider, parent));
        }
    }

    public class BaseMenu : ToolStripMenuItem
    {
        protected MenuStrip MenuStrip { get; set; }

        protected IServiceProvider ServiceProvider { get; set; }

        protected EditScriptForm Parent { get; set; }


        public BaseMenu(MenuStrip menuStrip, IServiceProvider serviceProvider, EditScriptForm parent)
        {
            this.MenuStrip = menuStrip;
            this.ServiceProvider = serviceProvider;
            this.Parent = parent;
        }
    }

    public class FileMenu : BaseMenu
    {
        public FileMenu(MenuStrip menuStrip, IServiceProvider serviceProvider, EditScriptForm parent) : base(menuStrip, serviceProvider, parent)
        {
            this.Text = "File";
            this.DropDownItems.Add(new FileOpen(menuStrip, serviceProvider, parent));
            this.DropDownItems.Add(new FileSave(menuStrip, serviceProvider, parent));


        }
    }

    public class FileSave : BaseMenu
    {
        public FileSave(MenuStrip menuStrip, IServiceProvider serviceProvider, EditScriptForm parent) : base(menuStrip, serviceProvider, parent)
        {
            this.Text = "Save";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Parent.SaveScript();
        }
    }

    public class FileOpen : BaseMenu
    {
        public FileOpen(MenuStrip menuStrip, IServiceProvider serviceProvider, EditScriptForm parent) : base(menuStrip, serviceProvider, parent)
        {
            this.Text = "Open";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Parent.OpenScript();
        }
    }

}
