using Cubase.Macro.Common.Socket;
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
            var socket = this.serviceProvider.GetRequiredService<CubaseMacroWebSocketClient>();

            var mainWindow = new Window(shell);
            mainWindow.Destroying += (s, o) => 
            {
                socket?.Close();
                socket?.Dispose();
            };
            return mainWindow;
        }
    }

}