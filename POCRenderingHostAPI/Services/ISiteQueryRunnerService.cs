using GraphQL.Common.Response;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Services
{
    public interface ISiteQueryRunnerService
    {
        Task<GraphQLEndpointResponse<SiteResponse>> GetSiteRoot(string siteName);
    }
}