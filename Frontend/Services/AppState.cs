using API.Contracts.User;
using Frontend.Security;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Frontend.Services
{
    public class AppState(JwtAuthenticationStateProvider authStateProvider, AccessTokenService accessTokenService, AuthService authService)
    {
        public UserResponse? User { get; private set; }

        private readonly JwtAuthenticationStateProvider _authStateProvider = authStateProvider;
        private readonly AccessTokenService _accessTokenService = accessTokenService;
        private readonly AuthService _authService = authService;

        public async Task InitializeUser()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user.Identity.IsAuthenticated)
            {
                var expiresStr = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

                User = new UserResponse(
                    int.Parse(user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value),
                    user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                    user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                    await _accessTokenService.Get(),
                    long.TryParse(expiresStr, out var expiresIn) ? DateTimeOffset.FromUnixTimeSeconds(expiresIn).UtcDateTime : null
                );

                Console.WriteLine($"User:\nId: {User.Id}\nUsername: {User.UserName}\nRole: {User.Role}\nToken: {User.Token}\nTokenExpires: {User.TokenExpires.ToString()}");
            }
        }

        public async Task Logout()
        {
            await _authService.Logout();
            User = null;
        }
    }
}
