using Newtonsoft.Json;

namespace POCRenderingHostAPI.Models
{
    public class CreateItemResponse
    {
        [JsonProperty("item")]
        public ItemResponse Item { get; set; }
    }
}
