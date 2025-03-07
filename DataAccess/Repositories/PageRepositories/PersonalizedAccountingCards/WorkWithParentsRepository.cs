using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.PersonalizedAccountingCards
{
    public class WorkWithParentsRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IWorkWithParentsRepository
    {
        public async Task<WorkWithParentsRecord?> CreateAsync(WorkWithParentsRecord record)
        {
            if (record == null) return null;
            if (!await CardExists(record.PersonalizedAccountingCardId)) return null;

            var createdRecord = await _dbContext.WorkWithParents.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.WorkWithParents.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<WorkWithParentsRecord>?> GetByCardIdAsync(int cardId)
        {
            if (!await CardExists(cardId)) return null;
            return await _dbContext.WorkWithParents.AsNoTracking()
                .Where(c => c.PersonalizedAccountingCardId == cardId).ToListAsync();
        }

        public async Task<WorkWithParentsRecord?> UpdateAsync(int id, WorkWithParentsRecord record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.WorkWithParents.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Date = record.Date;
            recordToUpdate.WorkContent = record.WorkContent;
            recordToUpdate.Note = record.Note;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> CardExists(int id) =>
            await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
