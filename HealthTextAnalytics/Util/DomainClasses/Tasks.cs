namespace HealthTextAnalytics.Util.DomainClasses;

public class Tasks
{
    public int completed { get; set; }
    public int failed { get; set; }
    public int inProgress { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

