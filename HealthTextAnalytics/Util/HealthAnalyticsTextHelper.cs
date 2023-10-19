using HealthTextAnalytics.Util.DomainClasses;
using System.Text;
using System.Text.Json;

namespace HealthTextAnalytics.Util;

public static class HealthAnalyticsTextHelper
{

    public static string CreateRequest(string inputText)
    {
        //note - the id 1 here in the request is a 'local id' that must be unique per request. only one text is supported in the 
        //request genreated, however the service allows multiple documents and id's if necessary. in this demo, we only will send in one text at a time
        var request = new
        {
            analysisInput = new
            {
                documents = new[]
                {
                    new { text = inputText, id = "1", language = "en" }
                }
            },
            tasks = new[]
            {
                new { id = "analyze 1", kind = "Healthcare", parameters = new { fhirVersion = "4.0.1" } }
            }
        };
        return JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
    }

    public static List<Entity> GetEntities(string analysisText)
    {
        try
        {
            Root doc = System.Text.Json.JsonSerializer.Deserialize<Root>(analysisText);

            //try loading up the documents inside of the analysisText
            var entities = doc?.tasks?.items.FirstOrDefault()?.results?.documents?.SelectMany(d => d.entities)?.ToList();
            return entities;
        }
        catch (Exception err)
        {
            Console.WriteLine("Got an error while trying to get entities: " + err.ToString());
        }
        return null;
    }

    public static string GetCategorizedInputText(string inputText, string analysisText)
    {
        var sb = new StringBuilder(inputText);
        try
        {
            Root doc = System.Text.Json.JsonSerializer.Deserialize<Root>(analysisText);

            //try loading up the documents inside of the analysisText
            var entities = doc?.tasks?.items.FirstOrDefault()?.results?.documents?.SelectMany(d => d.entities)?.ToList();
            if (entities != null)
            {
                foreach (var row in entities.OrderByDescending(r => r.offset))
                {
                    sb.Insert(row.offset + row.length, "</b></span>");
                    sb.Insert(row.offset, $"<span style='color:{GetBackgroundColor(row)}' title='{row.category}: {row.text} Confidence: {row.confidenceScore} {row.name}'><b>");
                }
            }
        }
        catch (Exception err)
        {

            Console.WriteLine("Got an error while trying to load in analysis healthcare json: " + err.ToString());
        }
        return $"<pre style='text-wrap:wrap; max-height:500px;font-size: 10pt;font-family:Verdana, Geneva, Tahoma, sans-serif;'>{sb}</pre>";
    }

    private static string GetBackgroundColor(Entity row)
    {
        var cat = row?.category?.ToLower();
        string backgroundColor = cat switch
        {
            "age" => "purple",
            "diagnosis" => "orange",
            "gender" => "purple",
            "symptomorsign" => "purple",
            "direction" => "blue",
            "symptom" => "purple",
            "symptoms" => "purple",
            "bodystructure" => "blue",
            "body" => "purple",
            "structure" => "purple",
            "examinationname" => "green",
            "procedure" => "green",
            "treatmentname" => "green",
            "conditionqualifier" => "lightgreen",
            "time" => "lightgreen",
            "date" => "lightgreen",
            "familyrelation" => "purple",
            "employment" => "purple",
            "livingstatus" => "purple",
            "administrativeevent" => "darkgreen",
            "careenvironment" => "darkgreen",
            _ => "darkgray"
        };
        return backgroundColor;
    }




}
