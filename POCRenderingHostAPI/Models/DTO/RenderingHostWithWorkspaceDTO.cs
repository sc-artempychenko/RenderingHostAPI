namespace POCRenderingHostAPI.Models.DTO
{
    public class RenderingHostWithWorkspaceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SiteName { get; set; }
        public string RepositoryUrl { get; set; }
        public string Host { get; set; }
        public string EnvironmentName { get; set; }
        public string RenderingHostUrl { get; set; }
        public string WorkspaceId { get; set; }
        public string WorkspaceUrl { get; set; }
        public HostingMethods RenderingHostHostingMethod { get; set; }
    }
}
