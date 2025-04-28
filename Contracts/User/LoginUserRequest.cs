using System.ComponentModel.DataAnnotations;

namespace Contracts.User
{
    public class LoginUserRequest(string username, string password)
    {
        [Required]
        public string Username { get; set; } = username;

        [Required]
        public string Password { get; set; } = password;
    }
}
