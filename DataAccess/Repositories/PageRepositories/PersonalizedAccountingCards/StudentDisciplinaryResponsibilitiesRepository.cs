using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.PersonalizedAccountingCards
{
    public class StudentDisciplinaryResponsibilitiesRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext),
        IStudentDisciplinaryResponsibilitiesRepository
    {
        public async Task<StudentDisciplinaryResponsibility?> CreateAsync(StudentDisciplinaryResponsibility record)
        {
            if (record == null) return null;
            if (!await CardExists(record.PersonalizedAccountingCardId)) return null;

            var createdRecord = await _dbContext.StudentDisciplinaryResponsibilities.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.StudentDisciplinaryResponsibilities.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentDisciplinaryResponsibility>?> GetByCardIdAsync(int cardId)
        {
            if (!await CardExists(cardId)) return null;
            return await _dbContext.StudentDisciplinaryResponsibilities.AsNoTracking()
                .Where(c => c.PersonalizedAccountingCardId == cardId).ToListAsync();
        }

        public async Task<StudentDisciplinaryResponsibility?> UpdateAsync(int id, StudentDisciplinaryResponsibility record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.StudentDisciplinaryResponsibilities.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Date = record.Date;
            recordToUpdate.Misdemeanor = record.Misdemeanor;
            recordToUpdate.DisciplinaryResponsibilityKind = record.DisciplinaryResponsibilityKind;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> CardExists(int id) =>
            await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
