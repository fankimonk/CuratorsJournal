using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class GroupActivesRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IGroupActivesRepository
    {
        public async Task<GroupActive?> CreateAsync(GroupActive active)
        {
            if (active == null) return null;
            if (active.StudentId != null && !await StudentExists((int)active.StudentId)) return null;
            if (!await PageExists(active.PageId)) return null;

            var createdActive = await _dbContext.GroupActives.AddAsync(active);

            await _dbContext.SaveChangesAsync();

            return createdActive.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.GroupActives.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<GroupActive>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.GroupActives.AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<GroupActive?> UpdateAsync(int id, GroupActive active)
        {
            if (active == null) return null;

            if (active.StudentId != null && !await StudentExists((int)active.StudentId)) return null;

            var activeToUpdate = await _dbContext.GroupActives.FirstOrDefaultAsync(p => p.Id == id);
            if (activeToUpdate == null) return null;

            activeToUpdate.PositionName = active.PositionName;
            activeToUpdate.StudentId = active.StudentId;

            await _dbContext.SaveChangesAsync();
            return activeToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.GroupActivesPage);

        private async Task<bool> StudentExists(int id) =>
            await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
