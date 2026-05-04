using Microsoft.Extensions.Logging;
using IptvSuite.Core.Services;
using IptvSuite.Mobile.MobileServices;

namespace IptvSuite.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // ✅ Core services
        builder.Services.AddSingleton(new HttpClient());

        // ✅ Providers (Xtream + Direct) + Registry
        builder.Services.AddSingleton<IStreamProvider, XtreamProvider>();
        builder.Services.AddSingleton<IStreamProvider, DirectUrlProvider>();
        builder.Services.AddSingleton<ProviderRegistry>();

        // ✅ App State
        builder.Services.AddSingleton<AppState>();

        return builder.Build();
    }
}