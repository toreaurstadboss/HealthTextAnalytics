using HealthTextAnalytics.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;

namespace HealthTextAnalytics.Util
{

    public class HealthAnalyticsTextClientService : IHealthAnalyticsTextClientService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private const int awaitTimeInMs = 500;
        private const int maxTimerWait = 10000;

        public HealthAnalyticsTextClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HealthTextAnalyticsResponse> GetHealthTextAnalytics(string inputText)
        {
            var client = _httpClientFactory.CreateClient("Az");
            string requestBodyRaw = HealthAnalyticsTextHelper.CreateRequest(inputText);
            //https://learn.microsoft.com/en-us/azure/ai-services/language-service/text-analytics-for-health/how-to/call-api?tabs=ner
            var stopWatch = Stopwatch.StartNew();
            HttpRequestMessage request = CreateTextAnalyticsRequest(requestBodyRaw);
            var response = await client.SendAsync(request);
            var result = new HealthTextAnalyticsResponse();
            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(awaitTimeInMs));
            int timeAwaited = 0;

            while (await timer.WaitForNextTickAsync())
            {
                if (response.IsSuccessStatusCode)
                {
                    result.IsSearchPerformed = true;
                    var operationLocation = response.Headers.First(h => h.Key?.ToLower() == Constants.Constants.HttpHeaderOperationResultAvailable).Value.FirstOrDefault();

                    var resultFromHealthAnalysis = await client.GetAsync(operationLocation);
                    JsonNode resultFromService = await resultFromHealthAnalysis.GetJsonFromHttpResponse();
                    if (resultFromService.GetValue<string>("status") == "succeeded")
                    {
                        result.AnalysisResultRawJson = await resultFromHealthAnalysis.Content.ReadAsStringAsync();
                        result.ExecutionTimeInMilliseconds = stopWatch.ElapsedMilliseconds;
                        result.Entities.AddRange(HealthAnalyticsTextHelper.GetEntities(result.AnalysisResultRawJson));
                        result.CategorizedInputText = HealthAnalyticsTextHelper.GetCategorizedInputText(inputText, result.AnalysisResultRawJson);
                        break;
                    }
                }
                timeAwaited += 500;
                if (timeAwaited >= maxTimerWait)
                {
                    result.CategorizedInputText = $"ERR: Timeout. Operation to analyze input text using Azure HealthAnalytics language service timed out after waiting for {timeAwaited} ms.";
                    break;
                }
            }

            return result;
        }

        private static HttpRequestMessage CreateTextAnalyticsRequest(string requestBodyRaw)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Constants.Constants.AnalyzeTextEndpoint);
            request.Content = new StringContent(requestBodyRaw, Encoding.UTF8, "application/json");//CONTENT-TYPE header
            return request;
        }
    }

}
