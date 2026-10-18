using Cubase.Macro.Registration;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Cubase.Macro.Tests
{
    public class BaseMidiTest
    {
        protected ServiceProvider serviceProvider;

        protected IConfigurationService configurationService;

        protected IMidiService midiService;

        protected IPLuginManagerService pluginManagerService;

        public BaseMidiTest()
        {
            var services = new ServiceCollection();
            services.RegisterCubaseMacro()
                    .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            this.serviceProvider = services.BuildServiceProvider();
            this.configurationService = serviceProvider.GetService<IConfigurationService>();
            this.midiService = serviceProvider.GetService<IMidiService>();
            this.pluginManagerService = serviceProvider.GetService<IPLuginManagerService>();
            // 

        }


    }
}
