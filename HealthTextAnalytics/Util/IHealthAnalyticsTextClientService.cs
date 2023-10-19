using HealthTextAnalytics.Models;

namespace HealthTextAnalytics.Util
{
    public interface IHealthAnalyticsTextClientService
    {
        Task<HealthTextAnalyticsResponse> GetHealthTextAnalytics(string inputText);
    }
}