namespace POCRenderingHostAPI.Models.DTO
{
    public class RenderingHostWithWorkspaceDTO
    {
        public string RenderingHostId { get; set; }
        public string Name { get; set; }
        public string SiteName { get; set; }
        public string SourceControlIntegrationName { get; set; }
        public string RepositoryUrl { get; set; }
        public string PlatformTenantName { get; set; }
        public string EnvironmentName { get; set; }
        public string RenderingHostUrl { get; set; }
        public string Status { get; set; }
        public string WorkspaceId { get; set; }
        public string WordspaceUrl { get; set; }
    }
}
