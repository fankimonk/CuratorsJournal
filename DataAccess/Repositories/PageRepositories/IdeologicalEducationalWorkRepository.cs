using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class IdeologicalEducationalWorkRepository(CuratorsJournalDBContext dBContext) 
        : PageRepositoryBase(dBContext), IIdeologicalEducationalWorkRepository
    {
        public async Task<CuratorsIdeologicalAndEducationalWorkAccountingRecord?> CreateAsync(
            CuratorsIdeologicalAndEducationalWorkAccountingRecord record)
        {
            if (record == null) return null;
            if (!await PageExists(record.PageId)) return null;

            var createdRecord = await _dbContext.CuratorsIdeologicalAndEducationalWorkAccounting.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.CuratorsIdeologicalAndEducationalWorkAccounting
                .Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CuratorsIdeologicalAndEducationalWorkAccountingRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.CuratorsIdeologicalAndEducationalWorkAccounting.AsNoTracking()
                .Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<CuratorsIdeologicalAndEducationalWorkAccountingRecord?> UpdateAsync(
            int id, CuratorsIdeologicalAndEducationalWorkAccountingRecord record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.CuratorsIdeologicalAndEducationalWorkAccounting.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.StartDay = record.StartDay;
            recordToUpdate.EndDay = record.EndDay;
            recordToUpdate.WorkContent = record.WorkContent;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.CuratorsIdeologicalAndEducationalWorkAccounting);
    }
}
