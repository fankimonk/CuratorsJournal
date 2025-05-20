namespace Diagrams.Frontend
{
    public class APIService(HttpClient httpClient, AccessTokenService accessTokenService, AuthService authService)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly AccessTokenService _accessTokenService = accessTokenService;
        private readonly AuthService _authService = authService;
    }
}
