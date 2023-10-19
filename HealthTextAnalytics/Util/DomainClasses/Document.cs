namespace HealthTextAnalytics.Util.DomainClasses;


#region Generated classes from sample Health analysis text

//Used this tool : https://json2csharp.com/

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Document
{
    public string id { get; set; }
    public List<Entity> entities { get; set; }
    public List<Relation> relations { get; set; }
    public List<object> warnings { get; set; }
}

#endregion
