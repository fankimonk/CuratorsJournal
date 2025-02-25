namespace Application.Helpers
{
    public class AuthorizationResult
    {
        public string Token { get; set; } = string.Empty;
        public string? Error { get; set; }

        private AuthorizationResult() { }

        public static AuthorizationResult UserNotFound =>
            new AuthorizationResult { Token = string.Empty, Error = "User not found" };

        public static AuthorizationResult WrongPassword =>
            new AuthorizationResult { Token = string.Empty, Error = "Wrong password" };

        public static AuthorizationResult Succeed(string token) =>
            new AuthorizationResult { Token = token, Error = null };
    }
}
