namespace POCRenderingHostAPI.Authentication
{
    internal class TokenProviderConfiguration
    {
        public TokenProviderConfiguration(string authority, string audience, string clientId, string clientSecret)
        {
            Authority = authority;
            Audience = audience;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string Authority { get; set; }

        public string Audience { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
