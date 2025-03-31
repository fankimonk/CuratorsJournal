using Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Frontend.Security
{
    public class JwtAuthenticationStateProvider(AccessTokenService accessTokenService) : AuthenticationStateProvider
    {
        private readonly AccessTokenService _accessTokenService = accessTokenService;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _accessTokenService.Get();
                if (string.IsNullOrEmpty(token))
                {
                    return await MarkAsUnauthorized();
                }

                var readJWT = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
                var principal = new ClaimsPrincipal(identity);

                var state = new AuthenticationState(principal);
                NotifyAuthenticationStateChanged(Task.FromResult(state));

                return await Task.FromResult(state);
            }
            catch (Exception e)
            {
                return await MarkAsUnauthorized();
            }
        }

        private async Task<AuthenticationState> MarkAsUnauthorized()
        {
            try
            {
                var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                NotifyAuthenticationStateChanged(Task.FromResult(state));

                return state;
            }
            catch (Exception e)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
