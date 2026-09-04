using Nutstone.Server.Common.Models;
using System.Text.Json;

namespace Cubase.Macro.Mobile.Configuration
{
    public class MobileConfiguration
    {
#if DEBUG
        public string MidiServerIpAddress { get; set; } = "192.168.1.110";
        // Place sensitive logging or diagnostic tools here

#else
        public string MidiServerIpAddress { get; set; } = "192.168.4.9";
#endif       

        public IEnumerable<string>? AvailableIpAddresses { get; set; } = ["192.168.1.110", "192.168.4.110", "192.168.4.9"];

        public string NutstoneServer { get; set; } = "https://nutstone.com";

        public AuthorisationKeys SecurityKeys { get; set; } =
            new AuthorisationKeys()
            {
                AuthHeader = "X-Api-Key",
                Read = "8C1A426E-5235-41CC-9B1C-595E0646B63F"
            };

        public string LyricDirectory { get; set; } = "Lyrics";

        public void Save()
        {
            var rootDirectory = Path.GetDirectoryName(CubaseMacroMobileConstants.ConfigurationFile);
            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }
            File.WriteAllText(CubaseMacroMobileConstants.ConfigurationFile, JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true }));
        }

        public static MobileConfiguration LoadFromFile()
        {
            if (File.Exists(CubaseMacroMobileConstants.ConfigurationFile))
            {
                return JsonSerializer.Deserialize<MobileConfiguration>(File.ReadAllText(CubaseMacroMobileConstants.ConfigurationFile));
            }
            return new MobileConfiguration();
        }
    }
}
