namespace Frontend.Utils.Auth
{
    public record AuthStatus
    (
        bool Status,
        string? ErrorMessage
    );
}
