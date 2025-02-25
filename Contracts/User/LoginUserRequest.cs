using System.ComponentModel.DataAnnotations;

namespace API.Contracts.User
{
    public record LoginUserRequest(
        [Required]
        string Username,

        [Required] 
        string Password
        );
}
