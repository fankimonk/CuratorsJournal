using Contracts.User;

namespace API.Contracts.User
{
    public record UserResponse(
        int Id,
        string UserName,
        RoleResponse Role,
        int? WorkerId,
        string? Token,
        DateTime? TokenExpires
    );
}
