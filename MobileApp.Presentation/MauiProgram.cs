using IdentityModel.OidcClient;
using Microsoft.Extensions.Logging;
using MobileApp.Presentation.Auth;
using MobileApp.Presentation.Data;

namespace MobileApp.Presentation
{
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

            
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<AuthService>();
            
            //builder.Services.AddSingleton<HttpClient>(GetHttpClient());
            builder.Services.AddTransient<WebAuthenticatorBrowser>();

            builder.Services.AddTransient<OidcClient>(sp =>
                new OidcClient(new OidcClientOptions
                {
                    Authority =  DeviceInfo.Platform == DevicePlatform.Android
                                    ? "https://10.0.2.2:8089"
                                    : "https://localhost:8089",
                    ClientId = "mobile-app",
                    Scope = "openid profile",
                    RedirectUri = "myapp://",
                    PostLogoutRedirectUri = "myapp://",
                    HttpClientFactory = options => GetHttpClient(),
                    Browser = sp.GetRequiredService<WebAuthenticatorBrowser>()
                }));

            return builder.Build();
        }

        public static HttpClient GetHttpClient()
        {
            var devSslHelper = new DevHttpsConnectionHelper();
            devSslHelper.HttpClient.Timeout = TimeSpan.FromSeconds(60);
            return devSslHelper.HttpClient;
        }
    }
}