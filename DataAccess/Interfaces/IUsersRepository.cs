using Domain.Enums;
using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> UsernameExistsAsync(string userName);
        Task<User?> GetByRefreshTokenId(int id);
        Task<User?> GetByRefreshToken(string token);
    }
}
