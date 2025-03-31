namespace Frontend.Services
{
    public class AccessTokenService(CookiesService cookiesService)
    {
        private readonly CookiesService _cookiesService = cookiesService;

        private readonly string _tokenKey = "access_token";

        public async Task Set(string accessToken)
        {
            await _cookiesService.Set(_tokenKey, accessToken, 1);
        }

        public async Task<string> Get()
        {
            return await _cookiesService.Get(_tokenKey);
        }

        public async Task Delete()
        {
            await _cookiesService.Delete(_tokenKey);
        }
    }
}
