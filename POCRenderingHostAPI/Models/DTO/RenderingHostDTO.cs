using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POCRenderingHostAPI.Models.DTO
{
    public class RenderingHostDTO
    {
        [Key]
        public string RenderingHostId { get; set; }
        public string Name { get; set; }
        public string SiteName { get; set; }
        public string SourceControlIntegrationName { get; set; }
        public string RepositoryUrl { get; set; }
        public string PlatformTenantName { get; set; }
        public string EnvironmentName { get; set; }
        [ForeignKey("Name")]
        public HostingMethods RenderingHostHostingMethod { get; set; }
        public string RenderingHostUrl { get; set; }
        public string DefinitionItemId { get; set; }
    }
}
