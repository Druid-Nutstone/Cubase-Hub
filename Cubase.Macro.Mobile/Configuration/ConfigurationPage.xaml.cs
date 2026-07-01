
namespace Cubase.Macro.Mobile.Configuration;

public partial class ConfigurationPage : ContentPage
{
	private readonly IMobileConfigurationService configurationService;

    public ConfigurationPage(IMobileConfigurationService configurationService)
	{
		InitializeComponent();
		this.configurationService = configurationService;
        SaveConfiguration.Clicked += async (s, e) =>
        {
            this.configurationService.Configuration.MidiServerIpAddress = ServerIPAddress.Text;
            this.configurationService.Configuration.Save();
            await DisplayAlertAsync("Configuration Saved", "Configuration has been saved successfully.", "OK");
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ServerIPAddress.Text = this.configurationService.Configuration.MidiServerIpAddress;
        IpAddressPicker.Items.Clear();
        IpAddressPicker.ItemsSource = (System.Collections.IList)this.configurationService.Configuration.AvailableIpAddresses;
        IpAddressPicker.SelectedIndexChanged += (s, e) => 
        {
            ServerIPAddress.Text = IpAddressPicker.SelectedItem.ToString();
        };
    }
}