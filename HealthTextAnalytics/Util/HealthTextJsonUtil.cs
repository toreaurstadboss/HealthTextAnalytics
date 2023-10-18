using System.Text;

namespace HealthTextAnalytics.Util
{
    public static class HealthTextJsonUtil
    {

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
            return $"<pre style='max-height:500px;font-size: 10pt;font-family:Verdana, Geneva, Tahoma, sans-serif;'>{sb}</pre>";
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

        public class Item
        {
            public string kind { get; set; }
            public DateTime lastUpdateDateTime { get; set; }
            public string status { get; set; }
            public Results results { get; set; }
        }

        public class Link
        {
            public string dataSource { get; set; }
            public string id { get; set; }
        }

        public class Relation
        {
            public double confidenceScore { get; set; }
            public string relationType { get; set; }
            public List<Entity> entities { get; set; }
        }

        public class Results
        {
            public List<Document> documents { get; set; }
            public List<object> errors { get; set; }
            public string modelVersion { get; set; }
        }

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

        public class Tasks
        {
            public int completed { get; set; }
            public int failed { get; set; }
            public int inProgress { get; set; }
            public int total { get; set; }
            public List<Item> items { get; set; }
        }






        #endregion


    }
}
