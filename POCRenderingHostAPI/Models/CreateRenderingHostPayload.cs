namespace POCRenderingHostAPI.Models
{
    public class CreateRenderingHostPayload
    {
        public string Id { get; set; }
        
        public HostingMethods RenderingHostHostingMethod { get; set; }
        
        public string Name { get; set; }
        
        public string RenderingHostUrl { get; set; }
        
        public string RepositoryUrl { get; set; }
        
        public string WorkspaceId { get; set; }
        
        public string WorkspaceUrl { get; set;}
        
        public string SiteName { get; set; }
        
        public string EnvironmentName { get; set; }
        
        public string EnvironmentId { get; set; }
        
        public string Host { get; set; }
    }
}
