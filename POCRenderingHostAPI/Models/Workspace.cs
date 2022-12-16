using Newtonsoft.Json;
using POCRenderingHostAPI.Converters;

namespace POCRenderingHostAPI.Models;

[JsonConverter(typeof(WorkspaceConverter))]
public class Workspace
{
    public string RenderingHostId { get; set; }
    public string UserEmail { get; set; }
    public string RenderingHostUrl { get; set; }
    public string WorkspaceId { get; set; }
    public string WorkspaceUrl { get; set; }
    public bool IsWorkspaceActive { get; set; }
    public bool IsRenderingHostActive { get; set; }
}

