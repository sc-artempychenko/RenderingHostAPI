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
    public class RenderingHostQueryRunnerService : IRenderingHostQueryRunnerService, IDisposable
    {
        private readonly GraphQLClientAuth _client;
        private readonly ITokenProvider _tokenProvider;
        private TokenResponse _jwtToken;
        private int _authorizationCounter = default;

        public RenderingHostQueryRunnerService(ITokenProvider tokenProvider)
        {
            _client = new GraphQLClient(new Uri("https://xmc-xmcloude2ehelix-resetsprint602e-pocrenderinf45b-s.sitecore-staging.cloud/sitecore/api/authoring/graphql/v1/"));
            _tokenProvider = tokenProvider;

            _jwtToken = _tokenProvider.RequestResourceOwnerPasswordAsync("", "").Result;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_jwtToken.AccessToken}");
            HandleSecurityProtocol();
        }

        public async Task<GraphQLEndpointResponse<CreateItemResponse>> CreateRenderingHostDefinitionItem(string rhName, string endpointUrl, string applicationUrl, 
            string appName)
        {
            if (_authorizationCounter > 1)
            {
                throw new InvalidOperationException("Authorization failed. Please check your credentials.");
            }

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
                                value = endpointUrl
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
                _authorizationCounter++;
                _jwtToken = await _tokenProvider.RequestResourceOwnerPasswordAsync("", "");
                if (_client.DefaultRequestHeaders.Authorization != null)
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                }
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_jwtToken.AccessToken}");

                result = await CreateRenderingHostDefinitionItem(rhName, endpointUrl, applicationUrl, appName);
            }

            _authorizationCounter = 0;
            return result;
        }

        public async Task<GraphQLEndpointResponse<UpdateItemResponse>> SwitchRenderingHostForSite(string rhName, string appName, string rootPath)
        {
            if (_authorizationCounter > 1)
            {
                throw new InvalidOperationException("Authorization failed. Please check your credentials.");
            }
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
                _authorizationCounter++;
                _jwtToken = await _tokenProvider.RequestResourceOwnerPasswordAsync("", "");
                if (_client.DefaultRequestHeaders.Authorization != null)
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                }
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_jwtToken.AccessToken}");

                result = await SwitchRenderingHostForSite(rhName, appName, rootPath);
            }

            _authorizationCounter = 0;
            return result;
        }

        public async Task<GraphQLEndpointResponse<DeleteItemResponse>> RemoveRenderingHost(string id)
        {
            if (_authorizationCounter > 1)
            {
                throw new InvalidOperationException("Authorization failed. Please check your credentials.");
            }
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
                _authorizationCounter++;
                _jwtToken = await _tokenProvider.RequestResourceOwnerPasswordAsync("", "");
                if (_client.DefaultRequestHeaders.Authorization != null)
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                }
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_jwtToken.AccessToken}");

                result = await RemoveRenderingHost(id);
            }

            _authorizationCounter = 0;
            return result;
        }

        public void Dispose()
        {
            _client.Dispose();
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
