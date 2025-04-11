using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend.Services
{
    public class APIService(HttpClient httpClient, AccessTokenService accessTokenService, AuthService authService)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly AccessTokenService _accessTokenService = accessTokenService;
        private readonly AuthService _authService = authService;

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

        public async Task<T?> GetFromJsonAsync<T>(string endpoint)
        {
            var token = await _accessTokenService.Get();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<T>(endpoint);
            if (response == null)
            {
                var refreshTokenResult = await _authService.RefreshTokenAsync();
                if (!refreshTokenResult) await _authService.Logout();

                var newToken = await _accessTokenService.Get();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                var newResponse = await _httpClient.GetFromJsonAsync<T>(endpoint);
                return newResponse;
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string endpoint, T obj)
        {
            var token = await _accessTokenService.Get();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync<T>(endpoint, obj);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshTokenResult = await _authService.RefreshTokenAsync();
                if (!refreshTokenResult) await _authService.Logout();

                var newToken = await _accessTokenService.Get();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                var newResponse = await _httpClient.PostAsJsonAsync<T>(endpoint, obj);
                return newResponse;
            }
            return response;
        }
    }
}
