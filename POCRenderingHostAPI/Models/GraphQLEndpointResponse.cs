using GraphQL.Common.Response;
using Newtonsoft.Json.Linq;

namespace POCRenderingHostAPI.Models
{
    public class GraphQLEndpointResponse<T>
    {
        // ReSharper disable once InconsistentNaming
        public T DTO { get; set; }
        public GraphQLError[] Errors { get; set; }
        public JToken Json { get; set; }
    }
}
