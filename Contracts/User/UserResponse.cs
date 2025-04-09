namespace API.Contracts.User
{
    public record UserResponse(
        int Id,
        string UserName,
        string Role,
        int? WorkerId,
        string? Token,
        DateTime? TokenExpires
    );
}
