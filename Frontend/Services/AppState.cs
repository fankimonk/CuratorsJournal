using API.Contracts.User;
using Contracts.User;
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
                var workerIdStr = user.Claims.FirstOrDefault(c => c.Type == "workerId")?.Value;

                User = new UserResponse(
                    int.Parse(user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value),
                    user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                    new RoleResponse(-1, user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value),
                    int.TryParse(workerIdStr, out var workerId) ? workerId : null,
                    await _accessTokenService.Get(),
                    long.TryParse(expiresStr, out var expiresIn) ? DateTimeOffset.FromUnixTimeSeconds(expiresIn).UtcDateTime : null
                );

                Console.WriteLine($"User:\nId: {User.Id}\n" +
                    $"Username: {User.UserName}\n" +
                    $"Role: {User.Role.Name}\n" +
                    $"WorkerId: {User.WorkerId}\n" +
                    $"Token: {User.Token}\n" +
                    $"TokenExpires: {User.TokenExpires}");
            }
        }

        public async Task Logout()
        {
            await _authService.Logout();
            User = null;
        }
    }
}
