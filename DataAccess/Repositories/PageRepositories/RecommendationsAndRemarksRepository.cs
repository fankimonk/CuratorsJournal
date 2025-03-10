using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class RecommendationsAndRemarksRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), 
        IRecommendationsAndRemarksRepository
    {
        public async Task<RecomendationsAndRemarksRecord?> CreateAsync(RecomendationsAndRemarksRecord record)
        {
            if (record == null) return null;
            if (!await PageExists(record.PageId)) return null;
            if (record.ReviewerId != null && !await WorkerExists((int)record.ReviewerId)) return null;

            var createdRecord = await _dbContext.RecomendationsAndRemarks.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.RecomendationsAndRemarks.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<RecomendationsAndRemarksRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.RecomendationsAndRemarks.AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<RecomendationsAndRemarksRecord?> UpdateAsync(int id, RecomendationsAndRemarksRecord record)
        {
            if (record == null) return null;
            if (record.ReviewerId != null && !await WorkerExists((int)record.ReviewerId)) return null;

            var recordToUpdate = await _dbContext.RecomendationsAndRemarks.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Date = record.Date;
            recordToUpdate.ExecutionDate = record.ExecutionDate;
            recordToUpdate.Content = record.Content;
            recordToUpdate.Result = record.Result;
            recordToUpdate.ReviewerId = record.ReviewerId;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.RecomendationsAndRemarksPage);

        private async Task<bool> WorkerExists(int id) =>
            await _dbContext.Workers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
