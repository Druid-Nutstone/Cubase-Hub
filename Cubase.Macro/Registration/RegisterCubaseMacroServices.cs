using Cubase.Hub.Forms;
using Cubase.Hub.Forms.Albums;
using Cubase.Hub.Forms.CompletedMixes;
using Cubase.Hub.Forms.Mixes;
using Cubase.Hub.Forms.Tracks;
using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Cubase;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Projects;
using Cubase.Hub.Services.Track;
using Cubase.Macro.Common.Lyrics.Services.Scrolling;
using Cubase.Macro.Common.Socket;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Plugins;
using Cubase.Macro.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Nutstone.AI.Assistant.Services.Plugins;

namespace Cubase.Macro.Registration
{
    public static class RegisterCubaseMacroServices
    {

        public static IServiceCollection RegisterCubaseMacro(this IServiceCollection services)
        {
            return services
                   .AddSingleton<IKeyboardService, KeyboardService>()
                   .AddSingleton<IWindowService, WindowService>()
                   .AddSingleton<IConfigurationService, ConfigurationService>()
                   .AddSingleton<IMidiService, MidiService>()
                   .AddSingleton<IScrollerService, ScrollerService>()
                   .AddSingleton<IPLuginManagerService, PluginManagerService>()
                   .AddSingleton<IPluginService, PluginService>()
                   .AddSingleton<CubaseMacroWebSocketClient>();
        }

        /*
                 public ManageAlbumsForm(IConfigurationService configurationService,
                                IDirectoryService directoryService,
                                ITrackService trackService,
                                IAlbumService albumService,
                                IMessageService messageService,
                                IServiceProvider serviceProvider,
                                CompletedMixesForm completedMixesForm,
                                ManageMixesForm manageMixesForm,
                                IProjectService projectService)
         */

        public static IServiceCollection RegisterCubaseHubServices(this IServiceCollection services)
        {
            return services
                   .AddSingleton<Cubase.Hub.Services.Config.IConfigurationService, Cubase.Hub.Services.Config.ConfigurationService>()
                   .AddSingleton<IDirectoryService, DirectoryService>()
                   .AddSingleton<ITrackService, TrackService>()
                   .AddSingleton<IAlbumService, AlbumService>()
                   .AddSingleton<IMessageService, MessageService>()
                   .AddSingleton<IAudioService, AudioService>()
                   .AddSingleton<ICubaseService, CubaseService>()
                   .AddTransient<CompletedMixesForm>()
                   .AddTransient<NewAlbumForm>()
                   .AddTransient<NewTrackForm>()
                   .AddTransient<ManageMixesForm>()
                   .AddSingleton<IProjectService, ProjectService>()
                   .AddTransient<ManageAlbumsForm>();

        }
    }
}
