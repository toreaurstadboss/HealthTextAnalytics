using Microsoft.Extensions.Logging;
using HealthTextAnalytics.Data;

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

		builder.Services.AddSingleton<WeatherForecastService>();


		var azureEndpoint = Environment.GetEnvironmentVariable("AZURE_COGNITIVE_SERVICES_LANGUAGE_SERVICE_ENDPOINT");
        var azureKey = Environment.GetEnvironmentVariable("AZURE_COGNITIVE_SERVICES_LANGUAGE_SERVICE_ENDPOINT");

        if (string.IsNullOrWhiteSpace(azureEndpoint))
		{
			throw new ArgumentNullException(nameof(azureEndpoint), "Missing system environment variable: AZURE_COGNITIVE_SERVICES_LANGUAGE_SERVICE_ENDPOINT");
		}
        if (string.IsNullOrWhiteSpace(azureKey))
        {
            throw new ArgumentNullException(nameof(azureKey), "Missing system environment variable: AZURE_COGNITIVE_SERVICES_LANGUAGE_SERVICE_ENDPOINT");
        }

        var azureEndpointHost = new Uri(azureEndpoint).Host;

        builder.Services.AddHttpClient("Az", httpClient =>
		{
			httpClient.BaseAddress = new Uri(new Uri(azureEndpointHost).GetLeftPart(UriPartial.Authority)); //https://stackoverflow.com/a/18708268/741368
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureKey);

        });

		return builder.Build();
	}
}
