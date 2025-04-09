using System.ComponentModel.DataAnnotations;

namespace Contracts.User
{
    public record LoginUserRequest(
        [Required]
        string Username,

        [Required] 
        string Password
        );
}
