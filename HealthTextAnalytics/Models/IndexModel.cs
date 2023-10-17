using System.ComponentModel.DataAnnotations;

namespace HealthTextAnalytics.Models
{
   
    public class IndexModel
    {
     
        [Required]
        public string InputText { get; set; }

        public string AnalysisResult { get; set; }

        public long ExecutionTime { get; set; }

    }

}
