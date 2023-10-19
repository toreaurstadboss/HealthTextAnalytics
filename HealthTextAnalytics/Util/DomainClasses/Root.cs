namespace HealthTextAnalytics.Util.DomainClasses;

public class Root
{
    public string jobId { get; set; }
    public DateTime lastUpdatedDateTime { get; set; }
    public DateTime createdDateTime { get; set; }
    public DateTime expirationDateTime { get; set; }
    public string status { get; set; }
    public List<object> errors { get; set; }
    public Tasks tasks { get; set; }
}
