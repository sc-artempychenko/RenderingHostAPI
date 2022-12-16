using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Converters
{
    public class CreateRenderingHostPayloadConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            dynamic data = JObject.Load(reader);
            
            var model = new CreateRenderingHostPayload
            {
                RenderingHostId = data.renderingHostId,
                RenderingHostHostingMethod = data.renderingHostHostingMethod,
                Name = data.name,
                RenderingHostUrl = data.renderingHostUrl,
                SourceControlIntegrationName = data.sourceControlIntegrationName,
                RepositoryUrl = data.repositoryUrl,
                WorkspaceId = data.workspaceId,
                WorkspaceUrl = data.workspaceUrl,
                SiteName = data.siteName,
                PlatformTenantName = data.platformTenantName,
                EnvironmentName = data.environmentName,
                EnvironmentId = data.environmentId
            };

            return model;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CreateRenderingHostPayload);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private string GetRenderingHostHostingMethod(dynamic data)
        {
            if (bool.Parse(data.local) == true)
            {
                return nameof(data.local);
            }

            if (bool.Parse(data.gitpod) == true)
            {
                return nameof(data.gitpod);
            }

            if (bool.Parse(data.external) == true)
            {
                return nameof(data.external);
            }

            return string.Empty;
        }
    }
}
