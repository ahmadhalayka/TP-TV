using IptvSuite.Core.Models;

namespace IptvSuite.Mobile.MobileServices;

public class AppState
{
    public ProviderConfig Config { get; private set; } = new ProviderConfig();

    public void SetConfig(ProviderConfig cfg)
    {
        Config = cfg.Normalize();
    }

    public bool HasConfig =>
        Config.Mode == ProviderMode.Xtream
            ? !string.IsNullOrWhiteSpace(Config.XtreamBaseUrl) && !string.IsNullOrWhiteSpace(Config.Username)
            : (!string.IsNullOrWhiteSpace(Config.DirectBaseUrl) || !string.IsNullOrWhiteSpace(Config.DirectTemplate));
}