using GraphQL.Client;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Services
{
    public interface ISiteQueryRunnerService
    {
        void SetGraphQlClient(GraphQLClient client);
        Task<GraphQLEndpointResponse<SiteResponse>> GetSiteRoot(string siteName);
    }
}