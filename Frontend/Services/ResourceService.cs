namespace Frontend.Services
{
    public class ResourceService(APIService apiService)
    {
        private readonly APIService _apiService = apiService;

        public async Task<bool> Verify()
        {
            var response = await _apiService.GetAsync("api/auth/verify");
            return response.IsSuccessStatusCode;
        }
    }
}
