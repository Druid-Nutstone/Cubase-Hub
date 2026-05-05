using Microsoft.Extensions.DependencyInjection;

namespace Cubase.Macro.Mobile
{
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;
        
        public App(IServiceProvider services)
        {
            this.serviceProvider = services;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var shell = this.serviceProvider.GetRequiredService<AppShell>();
            return new Window(shell);
        }
    }
}