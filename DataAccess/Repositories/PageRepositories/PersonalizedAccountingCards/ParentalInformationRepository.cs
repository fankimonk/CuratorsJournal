using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.PersonalizedAccountingCards
{
    public class ParentalInformationRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IParentalInformationRepository
    {
        public async Task<ParentalInformationRecord?> CreateAsync(ParentalInformationRecord record)
        {
            if (record == null) return null;
            if (!await CardExists(record.PersonalizedAccountingCardId)) return null;

            var createdRecord = await _dbContext.ParentalInformation.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.ParentalInformation.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ParentalInformationRecord>?> GetByCardIdAsync(int cardId)
        {
            if (!await CardExists(cardId)) return null;
            return await _dbContext.ParentalInformation.AsNoTracking()
                .Where(c => c.PersonalizedAccountingCardId == cardId).ToListAsync();
        }

        public async Task<ParentalInformationRecord?> UpdateAsync(int id, ParentalInformationRecord record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.ParentalInformation.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.FirstName = record.FirstName;
            recordToUpdate.MiddleName = record.MiddleName;
            recordToUpdate.LastName = record.LastName;
            recordToUpdate.PlaceOfResidence = record.PlaceOfResidence;
            recordToUpdate.PlaceOfWork = record.PlaceOfWork;
            recordToUpdate.Position = record.Position;
            recordToUpdate.HomePhoneNumber = record.HomePhoneNumber;
            recordToUpdate.WorkPhoneNumber = record.WorkPhoneNumber;
            recordToUpdate.MobilePhoneNumber = record.MobilePhoneNumber;
            recordToUpdate.OtherInformation = record.OtherInformation;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> CardExists(int id) => 
            await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
