using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent.Literature;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class LiteratureWorkRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), ILiteratureWorkRepository
    {
        public async Task<LiteratureWorkRecord?> CreateAsync(LiteratureWorkRecord record)
        {
            if (record == null) return null;
            if (!await PageExists(record.PageId)) return null;
            if (record.LiteratureId != null && !await LiteratureExists((int)record.LiteratureId)) return null;

            var createdRecord = await _dbContext.LiteratureWork.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.LiteratureWork.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<LiteratureWorkRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.LiteratureWork.AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<LiteratureWorkRecord?> UpdateAsync(int id, LiteratureWorkRecord record)
        {
            if (record == null) return null;
            if (record.LiteratureId != null && !await LiteratureExists((int)record.LiteratureId)) return null;

            var recordToUpdate = await _dbContext.LiteratureWork.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.ShortAnnotation = record.ShortAnnotation;
            recordToUpdate.LiteratureId = record.LiteratureId;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.LiteratureWorkPage);

        private async Task<bool> LiteratureExists(int id) =>
            await _dbContext.LiteratureList.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
