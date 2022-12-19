using GraphQL.Client;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Services
{
    public interface IRenderingHostQueryRunnerService
    {
        void SetGraphQlClient(GraphQLClient client);
        Task<GraphQLEndpointResponse<CreateItemResponse>> CreateRenderingHostDefinitionItem(string rhName, string endpointUrl, string applicationUrl, string appName);
        Task<GraphQLEndpointResponse<UpdateItemResponse>> SwitchRenderingHostForSite(string rhName, string appName, string rootPath);
        Task<GraphQLEndpointResponse<DeleteItemResponse>> RemoveRenderingHost(string id);
    }
}