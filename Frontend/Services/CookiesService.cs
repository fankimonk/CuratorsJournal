using Microsoft.JSInterop;

namespace Frontend.Services
{
    public class CookiesService(IJSRuntime jsRuntime)
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        public async Task<string> Get(string key)
        {
            return await _jsRuntime.InvokeAsync<string>("getCookie", key);
        }

        public async Task Delete(string key)
        {
            await _jsRuntime.InvokeVoidAsync("deleteCookie", key);
        }

        public async Task Set(string key, string value, int days)
        {
            await _jsRuntime.InvokeVoidAsync("setCookie", key, value, days);
        }
    }
}
