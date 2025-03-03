using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class ParticipationInPedagogicalSeminarsRepository(CuratorsJournalDBContext dBContext) 
        : PageRepositoryBase(dBContext), IParticipationInPedagogicalSeminarsRepository
    {
        public async Task<CuratorsParticipationInPedagogicalSeminarsRecord?> CreateAsync(
            CuratorsParticipationInPedagogicalSeminarsRecord record)
        {
            if (record == null) return null;
            if (!await PageExists(record.PageId)) return null;

            var createdRecord = await _dbContext.CuratorsParticipationInPedagogicalSeminars.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.CuratorsParticipationInPedagogicalSeminars.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CuratorsParticipationInPedagogicalSeminarsRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.CuratorsParticipationInPedagogicalSeminars.AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<CuratorsParticipationInPedagogicalSeminarsRecord?> UpdateAsync(int id, 
            CuratorsParticipationInPedagogicalSeminarsRecord record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.CuratorsParticipationInPedagogicalSeminars.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Date = record.Date;
            recordToUpdate.Topic = record.Topic;
            recordToUpdate.ParticipationForm = record.ParticipationForm;
            recordToUpdate.SeminarLocation = record.SeminarLocation;
            recordToUpdate.Note = record.Note;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.CuratorsParticipationInPedagogicalSeminarsPage);
    }
}
