using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class StudentHealthCardRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IStudentHealthCardRepository
    {
        public async Task<StudentsHealthCardRecord?> CreateAsync(StudentsHealthCardRecord record)
        {
            if (record == null) return null;
            if (!await PageExists(record.PageId)) return null;
            if (record.StudentId != null && !await StudentExists((int)record.StudentId)) return null;

            var createdRecord = await _dbContext.StudentsHealthCards.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.StudentsHealthCards.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentsHealthCardRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.StudentsHealthCards.AsNoTracking()
                .Include(s => s.Page).ThenInclude(s => s.HealthCardPageAttributes)
                .Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<StudentsHealthCardRecord?> UpdateAsync(int id, StudentsHealthCardRecord record)
        {
            if (record == null) return null;
            if (record.StudentId != null && !await StudentExists((int)record.StudentId)) return null;

            var recordToUpdate = await _dbContext.StudentsHealthCards.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Number = record.Number;
            recordToUpdate.MissedClasses = record.MissedClasses;
            recordToUpdate.Note = record.Note;
            recordToUpdate.StudentId = record.StudentId;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.StudentsHealthCardPage);

        private async Task<bool> StudentExists(int id) =>
            await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
