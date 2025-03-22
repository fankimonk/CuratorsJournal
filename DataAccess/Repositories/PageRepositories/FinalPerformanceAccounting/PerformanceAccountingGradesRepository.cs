using DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting;
using Domain.Entities.JournalContent.FinalPerformanceAccounting;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.FinalPerformanceAccounting
{
    public class PerformanceAccountingGradesRepository(CuratorsJournalDBContext dBContext) : RepositoryBase(dBContext), IPerformanceAccountingGradesRepository
    {
        public async Task<List<PerformanceAccountingGrade>?> GetByColumnIdAsync(int columnId)
        {
            if (!await ColumnExists(columnId)) return null;
                return await _dbContext.PerformanceAccountingGrades.AsNoTracking().Where(c => c.PerformanceAccountingColumnId == columnId).ToListAsync();
        }

        public async Task<List<PerformanceAccountingGrade>?> GetByRecordIdAsync(int recordId)
        {
            if (!await RecordExists(recordId)) return null;
                return await _dbContext.PerformanceAccountingGrades.AsNoTracking().Where(c => c.PerformanceAccountingRecordId == recordId).ToListAsync();
        }

        public async Task<PerformanceAccountingGrade?> UpdateAsync(int id, PerformanceAccountingGrade grade)
        {
            if (grade == null) return null;

            var toUpdate = await _dbContext.PerformanceAccountingGrades.FirstOrDefaultAsync(p => p.Id == id);
            if (toUpdate == null) return null;

            toUpdate.IsPassed = grade.IsPassed;
            toUpdate.Grade = grade.Grade;

            await _dbContext.SaveChangesAsync();
            return toUpdate;
        }

        private async Task<bool> ColumnExists(int id) =>
            await _dbContext.PerformanceAccountingColumns.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> RecordExists(int id) =>
            await _dbContext.FinalPerformanceAccounting.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
