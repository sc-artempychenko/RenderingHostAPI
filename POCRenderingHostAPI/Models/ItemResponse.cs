using Newtonsoft.Json;

namespace POCRenderingHostAPI.Models
{
    public class ItemResponse
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("fields")]
        public FieldNodesResponse Fields { get; set; }
    }
}
