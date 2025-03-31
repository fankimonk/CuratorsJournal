using Domain.Entities.Auth;

namespace DataAccess.Interfaces
{
    public interface IRefreshTokensRepository
    {
        Task<RefreshToken?> AddAsync(RefreshToken token);
        Task<bool> DisableTokensByUserIdAsync(int userId);
        Task<bool> DisableTokenById(int id);
        Task<bool> DisableToken(string token);
        Task<bool> IsTokenValid(int id);
        Task<bool> IsTokenValid(string token);
    }
}
