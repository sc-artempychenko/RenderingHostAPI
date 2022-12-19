using Newtonsoft.Json;

namespace POCRenderingHostAPI.Models
{
    public class ItemResponse
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }
    }
}
