using Application.Entities;

namespace Application.Helpers
{
    public class AuthorizationResult
    {
        public AuthToken? Token { get; set; }
        public string? Error { get; set; }

        private AuthorizationResult() { }

        public static AuthorizationResult UserNotFound =>
            new AuthorizationResult { Token = null, Error = "User not found" };

        public static AuthorizationResult WrongPassword =>
            new AuthorizationResult { Token = null, Error = "Wrong password" };

        public static AuthorizationResult Succeed(AuthToken token) =>
            new AuthorizationResult { Token = token, Error = null };
    }
}
