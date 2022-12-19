using GraphQL.Client;
using System.Net;

namespace POCRenderingHostAPI.Services
{
    public class HostConfigurationProvider : IDisposable, IHostConfigurationProvider
    {
        private GraphQLClient _client;
        public string HostName { get; set; }

        public GraphQLClient GetGraphQlClient(string hostName)
        {
            HostName = hostName;
            var graphQlApi = $"https://{hostName}/sitecore/api/authoring/graphql/v1/";
            _client = new GraphQLClient(new Uri(graphQlApi));
            
            HandleSecurityProtocol();

            return _client;
        }

        public void SetJwtToken(string jwtToken)
        {
            if (_client.DefaultRequestHeaders.Authorization != null)
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
            }
            _client.DefaultRequestHeaders.Add("Authorization", jwtToken);
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
    }
}
