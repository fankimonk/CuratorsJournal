using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class EducationalProcessScheduleRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IEducationalProcessScheduleRepository
    {
        public async Task<EducationalProcessScheduleRecord?> CreateAsync(EducationalProcessScheduleRecord record)
        {
            if (record == null) return null;
            if (!await PageExists(record.PageId)) return null;

            var createdRecord = await _dbContext.EducationalProcessSchedule.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.EducationalProcessSchedule.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<EducationalProcessScheduleRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.EducationalProcessSchedule.AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<EducationalProcessScheduleRecord?> UpdateAsync(int id, EducationalProcessScheduleRecord record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.EducationalProcessSchedule.FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.SemesterNumber = record.SemesterNumber;
            recordToUpdate.StartDate = record.StartDate;
            recordToUpdate.EndDate = record.EndDate;
            recordToUpdate.SessionStartDate = record.SessionStartDate;
            recordToUpdate.SessionEndDate = record.SessionEndDate;
            recordToUpdate.PracticeStartDate = record.PracticeStartDate;
            recordToUpdate.PracticeEndDate = record.PracticeEndDate;
            recordToUpdate.VacationStartDate = record.VacationStartDate;
            recordToUpdate.VacationEndDate = record.VacationEndDate;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.EducationalProcessSchedule);
    }
}
