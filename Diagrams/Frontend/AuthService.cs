namespace Diagrams.Frontend
{
    public class AuthService(AccessTokenService accessTokenService, HttpClient httpClient, RefreshTokenService refreshTokenService)
    {
        private readonly RefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly AccessTokenService _accessTokenService = accessTokenService;
        private readonly HttpClient _httpClient = httpClient;
    }
}
