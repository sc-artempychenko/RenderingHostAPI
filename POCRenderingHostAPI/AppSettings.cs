namespace POCRenderingHostAPI
{
    public class AppSettings
    {
        private static IConfigurationRoot appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }
}
