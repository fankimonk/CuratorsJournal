using DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting;
using Domain.Entities.JournalContent.FinalPerformanceAccounting;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.FinalPerformanceAccounting
{
    public class PerformanceAccountingRecordsRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IPerformanceAccountingRecordsRepository
    {
        public async Task<FinalPerformanceAccountingRecord?> CreateAsync(FinalPerformanceAccountingRecord record)
        {
            if (record == null) return null;
            if (record.StudentId != null && !await StudentExists((int)record.StudentId)) return null;
            if (!await PageExists(record.PageId)) return null;

            var created = await _dbContext.FinalPerformanceAccounting.AddAsync(record);

            await _dbContext.PerformanceAccountingColumns
                .Where(r => r.PageId == record.PageId)
                .ForEachAsync(r => r.PerformanceAccountingGrades.Add(new PerformanceAccountingGrade
                {
                    PerformanceAccountingRecord = created.Entity,
                    PerformanceAccountingColumn = r
                }));

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.FinalPerformanceAccounting.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<FinalPerformanceAccountingRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.FinalPerformanceAccounting
                .Include(r => r.PerformanceAccountingGrades)
                .AsNoTracking()
                .Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<FinalPerformanceAccountingRecord?> UpdateAsync(int id, FinalPerformanceAccountingRecord record)
        {
            if (record == null) return null;

            if (record.StudentId != null && !await StudentExists((int)record.StudentId)) return null;

            var toUpdate = await _dbContext.FinalPerformanceAccounting.FirstOrDefaultAsync(p => p.Id == id);
            if (toUpdate == null) return null;

            toUpdate.Number = record.Number;
            toUpdate.StudentId = record.StudentId;

            await _dbContext.SaveChangesAsync();
            return toUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.FinalPerformanceAccounting);

        private async Task<bool> StudentExists(int id) =>
            await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
