using System.Text.Json.Serialization;

namespace POCRenderingHostAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HostingMethods
    {
        Local,
        Gitpod,
        External
    }
}
