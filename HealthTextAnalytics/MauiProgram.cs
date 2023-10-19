using Microsoft.Extensions.Logging;

namespace HealthTextAnalytics;

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

        var azureEndpoint = Environment.GetEnvironmentVariable("AZURE_COGNITIVE_SERVICES_LANGUAGE_SERVICE_ENDPOINT");
        var azureKey = Environment.GetEnvironmentVariable("AZURE_COGNITIVE_SERVICES_LANGUAGE_SERVICE_KEY");

        if (string.IsNullOrWhiteSpace(azureEndpoint))
        {
            throw new ArgumentNullException(nameof(azureEndpoint), "Missing system environment variable: AZURE_COGNITIVE_SERVICES_LANGUAGE_SERVICE_ENDPOINT");
        }
        if (string.IsNullOrWhiteSpace(azureKey))
        {
            throw new ArgumentNullException(nameof(azureKey), "Missing system environment variable: AZURE_COGNITIVE_SERVICES_LANGUAGE_SERVICE_KEY");
        }

        var azureEndpointHost = new Uri(azureEndpoint);

        builder.Services.AddHttpClient("Az", httpClient =>
        {
            string baseUrl = azureEndpointHost.GetLeftPart(UriPartial.Authority); //https://stackoverflow.com/a/18708268/741368
            httpClient.BaseAddress = new Uri(baseUrl);
            //httpClient..Add("Content-type", "application/json");
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureKey);
        });

        return builder.Build();
    }
}
