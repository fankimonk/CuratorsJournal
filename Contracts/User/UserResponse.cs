namespace API.Contracts.User
{
    public record UserResponse(
        int Id,
        string UserName,
        string Role,
        string? Token,
        DateTime? TokenExpires
    );
}
