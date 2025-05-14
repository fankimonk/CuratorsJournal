using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;

namespace Frontend.Services
{
    public class APIService(HttpClient httpClient, AccessTokenService accessTokenService, AuthService authService,
        NavigationManager navigationManager)
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
                if (newResponse.StatusCode == HttpStatusCode.Unauthorized) _navigationManager.NavigateTo("/login");
                return newResponse;
            }
            return response;
        }

        public async Task<T?> GetFromJsonAsync<T>(string endpoint)
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
                if (newResponse.StatusCode == HttpStatusCode.Unauthorized) _navigationManager.NavigateTo("/login");
                return await newResponse.Content.ReadFromJsonAsync<T>();
            }
            return await response.Content.ReadFromJsonAsync<T>();
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
                if (newResponse.StatusCode == HttpStatusCode.Unauthorized) _navigationManager.NavigateTo("/login");
                return newResponse;
            }
            return response;
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string endpoint, T obj)
        {
            var token = await _accessTokenService.Get();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync<T>(endpoint, obj);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshTokenResult = await _authService.RefreshTokenAsync();
                if (!refreshTokenResult) await _authService.Logout();

                var newToken = await _accessTokenService.Get();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                var newResponse = await _httpClient.PutAsJsonAsync<T>(endpoint, obj);
                if (newResponse.StatusCode == HttpStatusCode.Unauthorized) _navigationManager.NavigateTo("/login");
                return newResponse;
            }
            return response;
        }
    }
}
