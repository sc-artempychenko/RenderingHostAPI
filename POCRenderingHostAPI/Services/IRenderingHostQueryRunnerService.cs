using GraphQL.Common.Response;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Services
{
    public interface IRenderingHostQueryRunnerService
    {
        Task<GraphQLEndpointResponse<CreateItemResponse>> CreateRenderingHostDefinitionItem(string rhName, string endpointUrl, string applicationUrl, string appName);
        Task<GraphQLEndpointResponse<UpdateItemResponse>> SwitchRenderingHostForSite(string rhName, string appName, string rootPath);
        Task<GraphQLEndpointResponse<DeleteItemResponse>> RemoveRenderingHost(string id);
    }
}