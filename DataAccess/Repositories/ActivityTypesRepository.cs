using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ActivityTypesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IActivityTypesRepository
    {
        public async Task<ActivityType?> CreateAsync(ActivityType activityType)
        {
            if (activityType == null) return null;

            var created = await _dbContext.ActivityTypes.AddAsync(activityType);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.ActivityTypes.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ActivityType>> GetAllAsync()
        {
            return await _dbContext.ActivityTypes.AsNoTracking().ToListAsync();
        }

        public async Task<ActivityType?> UpdateAsync(int id, ActivityType activityType)
        {
            if (activityType == null) return null;

            var typeToUpdate = await _dbContext.ActivityTypes.FirstOrDefaultAsync(p => p.Id == id);
            if (typeToUpdate == null) return null;

            typeToUpdate.Name = activityType.Name;

            await _dbContext.SaveChangesAsync();
            return typeToUpdate;
        }
    }
}
