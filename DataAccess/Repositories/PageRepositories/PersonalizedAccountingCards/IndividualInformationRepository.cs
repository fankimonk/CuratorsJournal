using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.PersonalizedAccountingCards
{
    public class IndividualInformationRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IIndividualInformationRepository
    {
        public async Task<IndividualInformationRecord?> CreateAsync(IndividualInformationRecord record)
        {
            if (record == null) return null;
            if (!await CardExists(record.PersonalizedAccountingCardId)) return null;
            if (record.ActivityTypeId != null && !await ActivityTypeExists((int)record.ActivityTypeId)) return null;

            var createdRecord = await _dbContext.IndividualInformation.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.IndividualInformation.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<IndividualInformationRecord>?> GetByCardIdAsync(int cardId)
        {
            if (!await CardExists(cardId)) return null;
            return await _dbContext.IndividualInformation.AsNoTracking()
                .Where(c => c.PersonalizedAccountingCardId == cardId).ToListAsync();
        }

        public async Task<IndividualInformationRecord?> UpdateAsync(int id, IndividualInformationRecord record)
        {
            if (record == null) return null;
            if (record.ActivityTypeId != null && !await ActivityTypeExists((int)record.ActivityTypeId)) return null;

            var recordToUpdate = await _dbContext.IndividualInformation.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.ActivityName = record.ActivityName;
            recordToUpdate.StartDate = record.StartDate;
            recordToUpdate.EndDate = record.EndDate;
            recordToUpdate.Result = record.Result;
            recordToUpdate.Note = record.Note;
            recordToUpdate.ActivityTypeId = record.ActivityTypeId;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        private async Task<bool> CardExists(int id) =>
            await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> ActivityTypeExists(int id) =>
            await _dbContext.ActivityTypes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
