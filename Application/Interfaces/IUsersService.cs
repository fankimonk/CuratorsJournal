using Application.Helpers;

namespace Application.Interfaces
{
    public interface IUsersService
    {
        Task<AuthorizationResult> Login(string userName, string password);
        Task<RegistrationResult> Register(string userName, string password);
    }
}