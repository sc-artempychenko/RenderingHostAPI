using Newtonsoft.Json;

namespace POCRenderingHostAPI.Models
{
    public class SiteResponse
    {
        [JsonProperty("rootPath")]
        public string RootPath { get; set; }
    }
}
