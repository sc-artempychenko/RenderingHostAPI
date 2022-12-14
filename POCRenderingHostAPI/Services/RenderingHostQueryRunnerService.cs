using FluentAssertions;
using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using Newtonsoft.Json.Linq;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Services
{
    public class RenderingHostQueryRunnerService : IRenderingHostQueryRunnerService, IDisposable
    {
        private GraphQLClient _client;

        public void SetGraphQlClient(GraphQLClient client)
        {
            _client = client;
        }

        public async Task<GraphQLEndpointResponse<CreateItemResponse>> CreateRenderingHostDefinitionItem(string rhName, string endpointUrl, string applicationUrl, 
            string appName)
        {
            var query = QueryHelper.BuildQuery(Constants.CreateRHDefinitionItem);
            var request = new GraphQLRequest
            {
                Query = query,
                Variables = new
                {
                    name = rhName,
                    templateId = "{BC71D442-3E4F-46BA-887C-746E54F9BB83}",
                    parent = "{895E25C4-12B2-4B2C-8233-82F3B79EEF53}",
                    language = "en",
                    fields = new[]
                        {
                            new
                            {
                                name = "ServerSideRenderingEngineEndpointUrl",
                                value = $"{endpointUrl}api/editing/render"
                            },
                            new
                            {
                                name = "ServerSideRenderingEngineApplicationUrl",
                                value = applicationUrl
                            },
                            new
                            {
                                name = "AppName",
                                value = appName
                            }
                        }
                }
            };

            var result = await ExecutePostQuery<CreateItemResponse>(request, Constants.CreateItemFieldName);
            if (result == null)
            {
                throw new InvalidDataException("Query execution failed with unknown reason. Please go somewhere and check something.");
            }

            if (result.Errors != null && result.Errors.Any(e => e.Message.Equals("The current user is not authorized to access this resource.")))
            {
                throw new UnauthorizedAccessException("Authorization failed. Please check your credentials.");
            }
            
            return result;
        }

        public async Task<GraphQLEndpointResponse<UpdateItemResponse>> SwitchRenderingHostForSite(string rhName, string appName, string rootPath)
        {
            var query = QueryHelper.BuildQuery(Constants.SwitchRenderingHost);
            var request = new GraphQLRequest
            {
                Query = query,
                Variables = new
                {
                    path = $"{rootPath}/Settings/Site Grouping/{appName}",
                    fields = new[]
                        {
                            new
                            {
                                name = "RenderingHost",
                                value = rhName
                            }
                        }
                }
            };

            var result = await ExecutePostQuery<UpdateItemResponse>(request, Constants.UpdateItemFieldName);
            if (result == null)
            {
                throw new InvalidDataException("Query execution failed with unknown reason. Please go somewhere and check something.");
            }

            if (result.Errors != null && result.Errors.Any(e => e.Message.Equals("The current user is not authorized to access this resource.")))
            {
                throw new UnauthorizedAccessException("Authorization failed. Please check your credentials.");
            }
            
            return result;
        }

        public async Task<GraphQLEndpointResponse<DeleteItemResponse>> RemoveRenderingHost(string id)
        {
            var query = QueryHelper.BuildQuery(Constants.RemoveItem);
            var request = new GraphQLRequest
            {
                Query = query,
                Variables = new
                {
                    id = id
                }
            };

            var result = await ExecutePostQuery<DeleteItemResponse>(request, Constants.DeleteItemFieldName);
            if (result == null)
            {
                throw new InvalidDataException("Query execution failed with unknown reason. Please go somewhere and check something.");
            }

            if (result.Errors != null && result.Errors.Any(e => e.Message.Equals("The current user is not authorized to access this resource.")))
            {
                throw new UnauthorizedAccessException("Authorization failed. Please check your credentials.");
            }
            
            return result;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private async Task<GraphQLEndpointResponse<T>> ExecutePostQuery<T>(GraphQLRequest request, string fieldName)
        {
            var (response, dto) = await ExecuteGraphQLRequest<T>(request, fieldName);
            var jToken = response.Data == null ? new JObject() : ((JObject)response.Data).SelectToken(fieldName);

            return new GraphQLEndpointResponse<T>
            {
                Errors = response.Errors,
                DTO = dto,
                Json = jToken
            };
        }

        private async Task<Tuple<GraphQLResponse, T>> ExecuteGraphQLRequest<T>(GraphQLRequest request, string fieldName)
        {
            var response = await _client.PostAsync(request);

            response.Should().NotBeNull("GraphQL response returns null");

            var dto = default(T);
            if (response.Data != null)
            {
                dto = response.GetDataFieldAs<T>(fieldName);
            }

            return new Tuple<GraphQLResponse, T>(response, dto);
        }
    }
}
