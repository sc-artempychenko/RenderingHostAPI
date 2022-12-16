using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Converters
{
    public class WorkspaceConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            dynamic data = JObject.Load(reader);

            var model = new Workspace
            {
                RenderingHostId = data.ideId,
                UserEmail = data.userEmail,
                WorkspaceUrl = data.workspaceUrl,
                RenderingHostUrl = data.renderingHostPublicUrl,
                IsWorkspaceActive = data.isWorkspaceActive,
                IsRenderingHostActive = data.isRenderingHostActive
            };

            return model;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Workspace);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
