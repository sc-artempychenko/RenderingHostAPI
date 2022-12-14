using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using IdentityModel.Client;

namespace POCRenderingHostAPI.Authentication
{
    internal class Auth0TokenProvider : ITokenProvider
    {
        private readonly TokenProviderConfiguration _config;

        private readonly ConcurrentDictionary<string, TokenResponse> _cache;

        public Auth0TokenProvider()
        {

            _config = new TokenProviderConfiguration("", "", "", "");
            _cache = new ConcurrentDictionary<string, TokenResponse>();
        }

        public async Task<TokenResponse> RequestResourceOwnerPasswordAsync(string userName, string password)
        {
            userName = userName ?? string.Empty;

            var token = _cache.GetOrAdd(userName, key => RequestJwtToken(userName, password));

            return await Task.FromResult(token).ConfigureAwait(false);
        }

        private TokenResponse RequestJwtToken(string username, string password)
        {
            using (var webClient = new WebClient())
            {
                var auth0Uri = new Uri($"{_config.Authority}/oauth/token");

                var requestParameters = string.IsNullOrEmpty(username)
                    ? GetClientCredentialsFlowParameters()
                    : GetPasswordFlowParameters(username, password);

                var responseBytes = webClient.UploadValues(auth0Uri, "POST", requestParameters);
                var rawTokenResponse = Encoding.UTF8.GetString(responseBytes);

                return rawTokenResponse == null ? null : new TokenResponse(rawTokenResponse);
            }
        }

        private NameValueCollection GetPasswordFlowParameters(string username, string password)
        {
            return new NameValueCollection
            {
                { "grant_type", "password" },
                { "username", username },
                { "password", password },
                { "audience", _config.Audience },
                { "client_id", _config.ClientId },
                { "client_secret", _config.ClientSecret }
            };
        }

        private NameValueCollection GetClientCredentialsFlowParameters()
        {
            return new NameValueCollection
            {
                { "grant_type", "client_credentials" },
                { "audience", _config.Audience },
                { "client_id", _config.ClientId },
                { "client_secret", _config.ClientSecret }
            };
        }
    }
}
