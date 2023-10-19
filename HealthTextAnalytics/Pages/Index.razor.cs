using HealthTextAnalytics.Models;
using HealthTextAnalytics.Util;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HealthTextAnalytics.Pages
{
    public partial class Index
    {

        private IndexModel Model = new();
        MarkupString ms = new();
        private bool isProcessing = false;
        private bool isSearchPerformed = false;   

        protected override void OnParametersSet()
        {
            Model.InputText = SampleData.Sampledata.SamplePatientTextNote2.CleanupAllWhiteSpace();
            StateHasChanged();
        }

        private void removeWhitespace(ChangeEventArgs args)
        {
            Model.InputText = args.Value?.ToString().CleanupAllWhiteSpace();
            StateHasChanged();
        }

        private async Task Submit()
        {
            try
            {
                ResetFieldsForBeforeSearch();

                HealthTextAnalyticsResponse response = await _healthAnalyticsTextClientService.GetHealthTextAnalytics(Model.InputText);
                Model.EntititesInAnalyzedResult = response.Entities;
                Model.ExecutionTime = response.ExecutionTimeInMilliseconds;
                Model.AnalysisResult = response.AnalysisResultRawJson;

                ms = new MarkupString(response.CategorizedInputText);

                //var client = _httpClientFactory.CreateClient("Az");

                //string requestBodyRaw = HealthAnalyticsTextHelper.CreateRequest(Model.InputText);

                ////https://learn.microsoft.com/en-us/azure/ai-services/language-service/text-analytics-for-health/how-to/call-api?tabs=ner

                //var stopWatch = Stopwatch.StartNew();

                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Constants.Constants.AnalyzeTextEndpoint);
                //request.Content = new StringContent(requestBodyRaw, Encoding.UTF8, "application/json");//CONTENT-TYPE header
                //var response = await client.SendAsync(request);
                //const int awaitTimeInMs = 500;
                //var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(awaitTimeInMs));
                //const int maxTimerWait = 10000;
                //int timeAwaited = 0;

                //while (await timer.WaitForNextTickAsync())
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        isSearchPerformed = true;
                //        var operationLocation = response.Headers.First(h => h.Key?.ToLower() == "operation-location").Value.FirstOrDefault();

                //        var resultFromHealthAnalysis = await client.GetAsync(operationLocation);
                //        JsonNode resultFromService = await resultFromHealthAnalysis.GetJsonFromHttpResponse();
                //        if (resultFromService.GetValue<string>("status") == "succeeded")
                //        {
                //            Model.AnalysisResult = await resultFromHealthAnalysis.Content.ReadAsStringAsync();
                //            Model.ExecutionTime = stopWatch.ElapsedMilliseconds;
                //            break;
                //        }
                //    }
                //    timeAwaited += 500;
                //    if (timeAwaited >= maxTimerWait)
                //    {
                //        Model.AnalysisResult = $"ERR: Timeout. Operation to analyze input text using Azure HealthAnalytics language service timed out after waiting for {timeAwaited} ms.";
                //        break;
                //    }
                //}
                //ms = new MarkupString(HealthAnalyticsTextHelper.GetCategorizedInputText(Model.InputText, Model.AnalysisResult));
                //Model.EntititesInAnalyzedResult.AddRange(HealthAnalyticsTextHelper.GetEntities(Model.AnalysisResult));
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally
            {
                ResetFieldsAfterSearch();
                StateHasChanged();
            }
        }

        private void ResetFieldsForBeforeSearch()
        {
            isProcessing = true;
            isSearchPerformed = false;
            ms = new MarkupString(string.Empty);
            Model.EntititesInAnalyzedResult.Clear();
            Model.AnalysisResult = string.Empty;
        }

        private void ResetFieldsAfterSearch()
        {
            isProcessing = false;
            isSearchPerformed = true;
        }

    }
}
