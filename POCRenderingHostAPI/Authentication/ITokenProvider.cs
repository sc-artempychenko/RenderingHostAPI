using IdentityModel.Client;

namespace POCRenderingHostAPI.Authentication
{
    public interface ITokenProvider
    {
        Task<TokenResponse> RequestResourceOwnerPasswordAsync(string userName, string password);
    }
}
