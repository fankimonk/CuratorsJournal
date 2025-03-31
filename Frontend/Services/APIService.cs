using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend.Services
{
    public class APIService(HttpClient httpClient, AccessTokenService accessTokenService, AuthService authService, NavigationManager navigationManager)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly AccessTokenService _accessTokenService = accessTokenService;
        private readonly AuthService _authService = authService;
        private readonly NavigationManager _navigationManager = navigationManager;

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var token = await _accessTokenService.Get();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(endpoint);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshTokenResult = await _authService.RefreshTokenAsync();
                if (!refreshTokenResult) await _authService.Logout();

                var newToken = await _accessTokenService.Get();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                var newResponse = await _httpClient.GetAsync(endpoint);
                return newResponse;
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object obj)
        {
            var token = await _accessTokenService.Get();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync(endpoint, obj);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshTokenResult = await _authService.RefreshTokenAsync();
                if (!refreshTokenResult) await _authService.Logout();

                var newToken = await _accessTokenService.Get();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                var newResponse = await _httpClient.PostAsJsonAsync(endpoint, obj);
                return newResponse;
            }
            return response;
        }
    }
}
