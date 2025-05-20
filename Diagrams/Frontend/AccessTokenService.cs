namespace Diagrams.Frontend
{
    public class AccessTokenService(CookiesService cookiesService)
    {
        private readonly CookiesService _cookiesService = cookiesService;
    }
}
