using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PEGroupsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IPEGroupsRepository
    {
        public async Task<PEGroup?> CreateAsync(PEGroup peGroup)
        {
            if (peGroup == null) return null;

            var created = await _dbContext.PEGroups.AddAsync(peGroup);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.PEGroups.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<PEGroup>> GetAllAsync()
        {
            return await _dbContext.PEGroups.AsNoTracking().ToListAsync();
        }

        public async Task<PEGroup?> UpdateAsync(int id, PEGroup peGroup)
        {
            if (peGroup == null) return null;

            var peGroupToUpdate = await _dbContext.PEGroups.FirstOrDefaultAsync(p => p.Id == id);
            if (peGroupToUpdate == null) return null;

            peGroupToUpdate.Name = peGroup.Name;

            await _dbContext.SaveChangesAsync();
            return peGroupToUpdate;
        }
    }
}
