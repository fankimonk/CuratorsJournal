using Application.Entities;
using Application.Helpers;

namespace Application.Interfaces
{
    public interface IUsersService
    {
        Task<AuthorizationResult> Login(string userName, string password);
        Task<RegistrationResult> Register(string userName, string password, int roleId, int? workerId);
        Task<AuthToken?> RefreshToken(string token);
        Task Logout(string token);
    }
}