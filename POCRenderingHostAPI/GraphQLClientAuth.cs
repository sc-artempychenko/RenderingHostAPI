using GraphQL.Client;

namespace POCRenderingHostAPI
{
    public class GraphQLClientAuth
    {
        public static GraphQLClient GetClient(string hostName)
        {
            var graphQlApi = $"{hostName}/sitecore/api/authoring/graphql/v1/";
            
            return new GraphQLClient(new Uri(hostName));
        }
    }
}
