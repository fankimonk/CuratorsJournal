using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Services
{
    public class RefreshTokenService(ProtectedLocalStorage protectedLocalStorage)
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage = protectedLocalStorage;

        private readonly string _key = "refresh_token";

        public async Task Set(string value)
        {
            await _protectedLocalStorage.SetAsync(_key, value);
        }

        public async Task<string> Get()
        {
            var result = await _protectedLocalStorage.GetAsync<string>(_key);
            if (result.Success) return result.Value!;

            return string.Empty;
        }

        public async Task Delete()
        {
            await _protectedLocalStorage.DeleteAsync(_key);
        }
    }
}
