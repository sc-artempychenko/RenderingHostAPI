using FluentAssertions;
using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using Newtonsoft.Json.Linq;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Services
{
    public class SiteQueryRunnerService : ISiteQueryRunnerService, IDisposable
    {
        private GraphQLClient _client;

        public void SetGraphQlClient(GraphQLClient client)
        {
            _client = client;
        }

        public async Task<GraphQLEndpointResponse<SiteResponse>> GetSiteRoot(string siteName)
        {
            var query = QueryHelper.BuildQuery(Constants.GetSiteRootPathQuery);
            var request = new GraphQLRequest
            {
                Query = query,
                Variables = new
                {
                    siteName = siteName
                }
            };

            var result = await ExecutePostQuery<SiteResponse>(request, Constants.SiteFieldName);
            if (result == null )
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
