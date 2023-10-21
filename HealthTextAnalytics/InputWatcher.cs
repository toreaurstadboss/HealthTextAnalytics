using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HealthTextAnalytics
{

    public class InputWatcher : ComponentBase
    {
        private EditContext editContext;

        [CascadingParameter]
        protected EditContext EditContext
        {
            get => editContext;
            set
            {
                editContext = value;
                EditContext.OnFieldChanged += async (sender, e) =>
                {
                    await FieldChanged.InvokeAsync(e.FieldIdentifier.FieldName);
                };
            }
        }

        [Parameter]
        public EventCallback<string> FieldChanged { get; set; }

        public bool Validate() => EditContext?.Validate() ?? false;

    }

}