using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class RefreshTokensRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IRefreshTokensRepository
    {
        public async Task<RefreshToken?> AddAsync(RefreshToken token)
        {
            if (token == null) return null;
            if (!await UserExists(token.UserId)) return null;

            var created = await _dbContext.RefreshTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DisableToken(string token)
        {
            var existingToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (existingToken == null) return false;

            existingToken.Enabled = false;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DisableTokenById(int id)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Id == id);
            if (token == null) return false;

            token.Enabled = false;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DisableTokensByUserIdAsync(int userId)
        {
            if (!await UserExists(userId)) return false;

            var tokens = _dbContext.RefreshTokens.Where(t => t.UserId == userId);
            foreach (var token in tokens)
            {
                token.Enabled = false;
            }
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsTokenValid(int id)
        {
            var token = await _dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (token == null) return false;

            return token.Enabled && token.Expires >= DateTime.UtcNow;
        }

        public async Task<bool> IsTokenValid(string token)
        {
            var existingToken = await _dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(t => t.Token == token);
            if (existingToken == null) return false;

            return existingToken.Enabled && existingToken.Expires >= DateTime.UtcNow;
        }

        private async Task<bool> UserExists(int id) =>
            await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id) != null;
    }
}
