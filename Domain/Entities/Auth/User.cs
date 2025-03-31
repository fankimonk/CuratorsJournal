using Domain.Entities.Auth;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public Worker? Worker { get; set; }
        public Student? Student { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
