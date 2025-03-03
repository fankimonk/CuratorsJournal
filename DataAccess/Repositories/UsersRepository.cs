using Domain.Enums;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UsersRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IUsersRepository
    {
        public async Task<User?> CreateAsync(User user)
        {
            var createdUser = await _dbContext.Users.AddAsync(user);
            if (createdUser == null) return null;
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == createdUser.Entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.Include(u => u.Role).AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.Include(u => u.Role).AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<HashSet<Permissions>> GetUserPermissions(int id)
        {
            var user = await _dbContext.Users.AsNoTracking()
                .Include(u => u.Role)
                .ThenInclude(r => r!.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return [];

            return user.Role!.Permissions.Select(p => (Permissions)p.Id).ToHashSet();
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UsernameExistsAsync(string userName)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName);
            return user != null;
        }
    }
}
