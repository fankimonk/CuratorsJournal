using Domain.Entities.Auth;

namespace Application.Entities
{
    public class AuthToken
    {
        public string AccessToken { get; set; } = string.Empty;

        public RefreshToken? RefreshToken { get; set; }
    }
}
