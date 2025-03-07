using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.PersonalizedAccountingCards
{
    public class StudentEncouragementsRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IStudentEncouragementsRepository
    {
        public async Task<StudentEcouragement?> CreateAsync(StudentEcouragement record)
        {
            if (record == null) return null;
            if (!await CardExists(record.PersonalizedAccountingCardId)) return null;

            var createdRecord = await _dbContext.StudentEcouragements.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.StudentEcouragements.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentEcouragement>?> GetByCardIdAsync(int cardId)
        {
            if (!await CardExists(cardId)) return null;
            return await _dbContext.StudentEcouragements.AsNoTracking()
                .Where(c => c.PersonalizedAccountingCardId == cardId).ToListAsync();
        }

        public async Task<StudentEcouragement?> UpdateAsync(int id, StudentEcouragement record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.StudentEcouragements.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Date = record.Date;
            recordToUpdate.Achievement = record.Achievement;
            recordToUpdate.EncouragementKind = record.EncouragementKind;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> CardExists(int id) =>
            await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
