namespace HealthTextAnalytics.Util.DomainClasses;

public class Relation
{
    public double confidenceScore { get; set; }
    public string relationType { get; set; }
    public List<Entity> entities { get; set; }
}
