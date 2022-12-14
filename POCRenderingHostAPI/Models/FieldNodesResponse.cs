using Newtonsoft.Json;

namespace POCRenderingHostAPI.Models
{
    public class FieldNodesResponse
    {
        [JsonProperty("nodes")]
        public NameValueNodeSetResponse[] NameValueNodeSet { get; set; }
    }
}
