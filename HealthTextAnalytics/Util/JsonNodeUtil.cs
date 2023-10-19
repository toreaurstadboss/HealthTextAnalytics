using System.Text.Json;
using System.Text.Json.Nodes;

namespace HealthTextAnalytics.Util
{
    public static class JsonNodeUtil
    {

        public static async Task<JsonNode> GetJsonFromHttpResponse(this HttpResponseMessage response)
        {
            var resultFromService = JsonSerializer.Deserialize<JsonNode>(await response.Content.ReadAsStringAsync());
            return resultFromService;
        }

        public static T? GetValue<T>(this JsonNode jsonNode, string key)
        {
            if (jsonNode == null)
            {
                return default;
            }
            return jsonNode[key] != null ? jsonNode[key].GetValue<T>() : default;
        }

    }
}
