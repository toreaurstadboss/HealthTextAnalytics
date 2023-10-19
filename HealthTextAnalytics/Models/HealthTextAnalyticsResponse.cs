using HealthTextAnalytics.Util.DomainClasses;

namespace HealthTextAnalytics.Models
{
 
    public class HealthTextAnalyticsResponse
    {

        public string CategorizedInputText { get; set; }

        public string AnalysisResultRawJson { get; set; }

        public long ExecutionTimeInMilliseconds { get; set; }

        public bool IsSearchPerformed { get; set; }

        public List<Entity> Entities { get; private set; } = new();

    }

}
