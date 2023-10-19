namespace HealthTextAnalytics.Util.DomainClasses;

public class Results
{
    public List<Document> documents { get; set; }
    public List<object> errors { get; set; }
    public string modelVersion { get; set; }
}

