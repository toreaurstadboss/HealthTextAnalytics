namespace HealthTextAnalytics.Util.DomainClasses;


public class Item
{
    public string kind { get; set; }
    public DateTime lastUpdateDateTime { get; set; }
    public string status { get; set; }
    public Results results { get; set; }
}


