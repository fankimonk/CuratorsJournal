using Domain.Entities;

namespace Application.Helpers
{
    public class RegistrationResult
    {
        public User? User { get; set; }
        public string? Error { get; set; }

        private RegistrationResult() { }

        public static RegistrationResult UsernameTaken =>
            new RegistrationResult { User = null, Error = "Username is taken" };

        public static RegistrationResult FailedToCreate =>
            new RegistrationResult { User = null, Error = "Failed to create user" };

        public static RegistrationResult Succeed(User user) =>
            new RegistrationResult { User = user, Error = null };
    }
}
