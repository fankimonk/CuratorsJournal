using API.Contracts.User;
using Contracts.User;
using Frontend.Utils.Auth;
using Microsoft.AspNetCore.Components;

namespace Frontend.Services
{
    public class AuthService(AccessTokenService accessTokenService, RefreshTokenService refreshTokenService, NavigationManager navigationManager, HttpClient httpClient)
    {
        private readonly AccessTokenService _accessTokenService = accessTokenService;
        private readonly RefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly NavigationManager _navigationManager = navigationManager;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<AuthStatus> Login(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync<LoginUserRequest>("api/auth/login", new LoginUserRequest(username, password));
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<AuthResponse>();

                await _accessTokenService.Delete();
                await _accessTokenService.Set(token!.AccessToken);
                await _refreshTokenService.Set(token!.RefreshToken);

                return new AuthStatus(true, null);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new AuthStatus(false, errorMessage);
            }
        }

        public async Task<bool> RefreshTokenAsync()
        {
            var refreshToken = await _refreshTokenService.Get();
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"refreshtoken={refreshToken}");

            var response = await _httpClient.PostAsync("api/auth/refresh", null);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<AuthResponse>();
                await _accessTokenService.Set(token!.AccessToken);
                await _refreshTokenService.Set(token.RefreshToken);

                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            var refreshToken = await _refreshTokenService.Get();
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"refreshtoken={refreshToken}");

            var response = await _httpClient.PostAsync("api/auth/logout", null);
            if (response.IsSuccessStatusCode)
            {
                await _accessTokenService.Delete();
                await _refreshTokenService.Delete();
                _navigationManager.NavigateTo("/login", forceLoad: true);
            }
        }
    }
}
