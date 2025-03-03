using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class StudentListRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IStudentListRepository
    {
        public async Task<StudentListRecord?> CreateAsync(StudentListRecord record)
        {
            if (record == null) return null;
            if (!await StudentExists(record.StudentId)) return null;
            if (record.PersonalizedAccountingCardId != null && !await CardExists((int)record.PersonalizedAccountingCardId)) return null;
            if (!await PageExists(record.PageId)) return null;

            var createdRecord = await _dbContext.StudentList.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.StudentList.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentListRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.StudentList.AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<StudentListRecord?> UpdateAsync(int id, StudentListRecord record)
        {
            if (record == null) return null;

            if (record.PersonalizedAccountingCardId != null)
            {
                if (!await CardExists((int)record.PersonalizedAccountingCardId)) return null;
                if (!await StudentsMatch(record.StudentId, (int)record.PersonalizedAccountingCardId)) return null;
            }

            var recordToUpdate = await _dbContext.StudentList.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Number = record.Number;
            recordToUpdate.PersonalizedAccountingCardId = record.PersonalizedAccountingCardId;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.StudentListPage);

        private async Task<bool> StudentExists(int id) =>
            await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> CardExists(int id) =>
            await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> StudentsMatch(int studentId, int cardId)
        {
            var card = await _dbContext.PersonalizedAccountingCards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == cardId);
            if (card == null) return false;

            return card.StudentId == studentId;
        }
    }
}
