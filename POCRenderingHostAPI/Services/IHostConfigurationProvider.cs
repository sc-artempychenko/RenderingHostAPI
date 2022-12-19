using GraphQL.Client;

namespace POCRenderingHostAPI.Services;

public interface IHostConfigurationProvider
{
    string HostName { get; set; }
    GraphQLClient GetGraphQlClient(string hostName);
    void SetJwtToken(string jwtToken);
}