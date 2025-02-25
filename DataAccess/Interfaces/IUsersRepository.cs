using Domain.Enums;
using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<HashSet<Permissions>> GetUserPermissions(int id);
        Task<User?> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> UsernameExistsAsync(string userName);
    }
}
