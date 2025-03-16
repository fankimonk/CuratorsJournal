using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PositionsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IPositionsRepository
    {
        public async Task<Position?> CreateAsync(Position position)
        {
            if (position == null) return null;

            var created = await _dbContext.Positions.AddAsync(position);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Positions.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Position>> GetAllAsync()
        {
            return await _dbContext.Positions.AsNoTracking().ToListAsync();
        }

        public async Task<Position?> UpdateAsync(int id, Position position)
        {
            if (position == null) return null;

            var positionToUpdate = await _dbContext.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (positionToUpdate == null) return null;

            positionToUpdate.Name = position.Name;

            await _dbContext.SaveChangesAsync();
            return positionToUpdate;
        }
    }
}
