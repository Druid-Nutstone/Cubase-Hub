using Cubase.Macro.Services.Lex;
using Microsoft.Extensions.DependencyInjection;

namespace Cubase.Macro.Forms.Scripts
{
    public class ScriptIndexControl : ListView
    {
        private readonly IServiceProvider serviceProvider;

        private IScriptService cLService;

        public ScriptIndexControl(IServiceProvider serviceProvider) : base()
        {
            this.Dock = DockStyle.Fill;
            this.serviceProvider = serviceProvider;
            this.View = View.Details;
            this.MultiSelect = false;
            this.DoubleBuffered = true;
            this.cLService = serviceProvider.GetService<IScriptService>();
            this.AddHeader("Script");
        }

        public void Initialise()
        {
            var scriptService = this.serviceProvider.GetService<IScriptService>();

        }

        public void AddHeader(string text)
        {
            this.Columns.Add(text);
        }
    }
}
