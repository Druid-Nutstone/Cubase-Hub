using Cubase.Macro.Common.Socket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cubase.Macro.Mobile
{
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<App> logger;

        public App(IServiceProvider services, ILogger<App> logger)
        {
            this.serviceProvider = services;
            this.logger = logger;
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.LogError("Unhandled exception occurred: {Exception}", e.ExceptionObject);
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