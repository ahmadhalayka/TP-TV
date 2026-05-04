namespace IptvSuite.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // ✅ MainPage يبقى BlazorWebView
        MainPage = new MainPage();
    }
}
