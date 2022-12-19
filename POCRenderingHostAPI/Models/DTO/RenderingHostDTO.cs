using System.ComponentModel.DataAnnotations;

namespace POCRenderingHostAPI.Models.DTO
{
    public class RenderingHostDTO
    {
        [Key]
        public int Id { get; set; }
        public string RenderingHostId { get; set; }
        public string Name { get; set; }
        public string SiteName { get; set; }
        public string RepositoryUrl { get; set; }
        public string PlatformTenantName { get; set; }
        public string EnvironmentName { get; set; }
        public HostingMethods RenderingHostHostingMethod { get; set; }
        public string RenderingHostUrl { get; set; }
        public string DefinitionItemId { get; set; }
        public string Host { get; set; }
        public string WorkspaceId { get; set; }
        public string WorkspaceUrl { get; set; }
    }
}
