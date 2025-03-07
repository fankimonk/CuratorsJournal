using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.PersonalizedAccountingCards
{
    public class IndividualWorkWithStudentRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IIndividualWorkWithStudentRepository
    {
        public async Task<IndividualWorkWithStudentRecord?> CreateAsync(IndividualWorkWithStudentRecord record)
        {
            if (record == null) return null;
            if (!await CardExists(record.PersonalizedAccountingCardId)) return null;

            var createdRecord = await _dbContext.IndividualWorkWithStudents.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.IndividualWorkWithStudents.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<IndividualWorkWithStudentRecord>?> GetByCardIdAsync(int cardId)
        {
            if (!await CardExists(cardId)) return null;
            return await _dbContext.IndividualWorkWithStudents.AsNoTracking()
                .Where(c => c.PersonalizedAccountingCardId == cardId).ToListAsync();
        }

        public async Task<IndividualWorkWithStudentRecord?> UpdateAsync(int id, IndividualWorkWithStudentRecord record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.IndividualWorkWithStudents.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Date = record.Date;
            recordToUpdate.WorkDoneAndRecommendations = record.WorkDoneAndRecommendations;
            recordToUpdate.Result = record.Result;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> CardExists(int id) =>
            await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
