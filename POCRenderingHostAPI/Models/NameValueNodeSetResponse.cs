using Newtonsoft.Json;

namespace POCRenderingHostAPI.Models
{
    public class NameValueNodeSetResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
