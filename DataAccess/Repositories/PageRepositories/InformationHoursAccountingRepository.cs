using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class InformationHoursAccountingRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IInformationHoursAccountingRepository
    {
        public async Task<InformationHoursAccountingRecord?> CreateAsync(InformationHoursAccountingRecord record)
        {
            if (record == null) return null;
            if (!await PageExists(record.PageId)) return null;

            var createdRecord = await _dbContext.InformationHoursAccounting.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.InformationHoursAccounting.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<InformationHoursAccountingRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.InformationHoursAccounting.AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<InformationHoursAccountingRecord?> UpdateAsync(int id, InformationHoursAccountingRecord record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.InformationHoursAccounting.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Date = record.Date;
            recordToUpdate.Topic = record.Topic;
            recordToUpdate.Note = record.Note;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.InformationHoursAccounting);
    }
}
