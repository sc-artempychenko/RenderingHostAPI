using FluentAssertions;
using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using POCRenderingHostAPI.Authentication;
using POCRenderingHostAPI.Models;
using System.Net;

namespace POCRenderingHostAPI.Services
{
    public class SiteQueryRunnerService : ISiteQueryRunnerService, IDisposable
    {
        private readonly GraphQLClient _client;
        private readonly ITokenProvider _tokenProvider;
        private TokenResponse _jwtToken;
        private int _authorizationCounter = default;

        public SiteQueryRunnerService(ITokenProvider tokenProvider)
        {
            //_client = new GraphQLClient(new Uri("https://xmc-xmcloude2ehelix-resetsprint602e-pocrenderinf45b-s.sitecore-staging.cloud/sitecore/api/authoring/graphql/v1/"));
            _client = GraphQLClientAuth.GetClient("");
            _tokenProvider = tokenProvider;

            _jwtToken = _tokenProvider.RequestResourceOwnerPasswordAsync("", "").Result;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_jwtToken.AccessToken}");
            HandleSecurityProtocol();
        }

        public async Task<GraphQLEndpointResponse<SiteResponse>> GetSiteRoot(string siteName, TokenResponse jwtToken)
        {
            if (_authorizationCounter > 1)
            {
                throw new InvalidOperationException("Authorization failed. Please check your credentials.");
            }
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
                _authorizationCounter++;
                _jwtToken = await _tokenProvider.RequestResourceOwnerPasswordAsync("", "");
                if (_client.DefaultRequestHeaders.Authorization != null)
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");                  
                }
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken.AccessToken}");

                result = await GetSiteRoot(siteName, jwtToken);
            }

            _authorizationCounter = 0;
            return result;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private void HandleSecurityProtocol()
        {
            if (ServicePointManager.SecurityProtocol != SecurityProtocolType.SystemDefault)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
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
