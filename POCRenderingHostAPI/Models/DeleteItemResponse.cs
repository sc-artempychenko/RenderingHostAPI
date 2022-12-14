using Newtonsoft.Json;

namespace POCRenderingHostAPI.Models
{
    public class DeleteItemResponse
    {
        [JsonProperty("successful")]
        public bool Successful { get; set; }
    }
}
