namespace API.Contracts.User
{
    public record AuthResponse(
        string AccessToken,
        string RefreshToken
    );
}
