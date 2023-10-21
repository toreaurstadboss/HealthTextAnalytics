using HealthTextAnalytics.Models;
using HealthTextAnalytics.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HealthTextAnalytics.Pages
{
    public partial class Index
    {

        private IndexModel Model = new();
        MarkupString ms = new();
        private bool isProcessing = false;
        private bool isSearchPerformed = false;   

        private InputWatcher inputWatcher = new InputWatcher();
        private bool isInvalid = false;

        private void FieldChanged(string fieldName)
        {
            isInvalid = !inputWatcher.Validate();
        }
        
        protected override void OnParametersSet()
        {
            Model.InputText = SampleData.Sampledata.SamplePatientTextNote2.CleanupAllWhiteSpace();
            StateHasChanged();
        }

        private void removeWhitespace(KeyboardEventArgs eventArgs)
        {
            Model.InputText = Model.InputText.CleanupAllWhiteSpace();
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
