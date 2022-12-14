using Newtonsoft.Json;

namespace POCRenderingHostAPI.Models
{
    public class UpdateItemResponse
    {
        [JsonProperty("item")]
        public ItemResponse Item { get; set; }
    }
}
