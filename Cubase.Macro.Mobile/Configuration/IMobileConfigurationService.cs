namespace Cubase.Macro.Mobile.Configuration
{
    public interface IMobileConfigurationService
    {
        MobileConfiguration Configuration { get; set; }
        Task InitialiseConfiguration();
    }
}
