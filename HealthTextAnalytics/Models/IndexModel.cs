﻿using System.ComponentModel.DataAnnotations;
using static HealthTextAnalytics.Util.HealthTextJsonUtil;

namespace HealthTextAnalytics.Models
{
   
    public class IndexModel
    {
        public IndexModel()
        {
            EntititesInAnalyzedResult = new();
        }

        [Required]
        public string InputText { get; set; }

        public string AnalysisResult { get; set; }

        public string CategorizedAnalysisResult { get; set; }

        public List<Entity> EntititesInAnalyzedResult { get; set; }

        public long ExecutionTime { get; set; }

    }

}
