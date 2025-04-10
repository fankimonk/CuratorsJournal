using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.User
{
    public class RegisterUserRequest(string username, string password, int? roleId, int? workerId)
    {
        [Required]
        [MinLength(5, ErrorMessage = "Username must have at least 5 characters")]
        [MaxLength(20, ErrorMessage = "Username must not have more than 20 characters")]
        public string Username { get; set; } = username;

        [Required]
        [MinLength(8, ErrorMessage = "Password must have at least 8 characters")]
        public string Password { get; set; } = password;

        [Required]
        [NotNull]
        public int? RoleId { get; set; } = roleId;

        public int? WorkerId { get; set; } = workerId;
    }
}
