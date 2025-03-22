using API.Contracts.User;

namespace Frontend.Services
{
    public class AppState(HttpClient httpClient)
    {
        public UserResponse? User { get; private set; }

        private readonly HttpClient _httpClient = httpClient;

        public async Task FetchUser()
        {
            var response = await _httpClient.GetAsync("api/auth/check");
            if (response.IsSuccessStatusCode)
            {
                User = await response.Content.ReadFromJsonAsync<UserResponse>();
                Console.WriteLine(User!.Id + " " + User.UserName + " " + User.Role);
            }
            else
            {
                Console.WriteLine("Not logged in");
            }
        }
    }
}
