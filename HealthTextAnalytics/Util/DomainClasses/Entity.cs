namespace HealthTextAnalytics.Util.DomainClasses;


public class Entity
{
    public int offset { get; set; }
    public int length { get; set; }
    public string text { get; set; }
    public string category { get; set; }
    public double confidenceScore { get; set; }
    public string name { get; set; }
    public List<Link> links { get; set; }
    public string @ref { get; set; }
    public string role { get; set; }
}

