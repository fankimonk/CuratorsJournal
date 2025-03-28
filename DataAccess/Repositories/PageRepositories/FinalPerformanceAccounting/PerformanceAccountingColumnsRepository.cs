using DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting;
using Domain.Entities;
using Domain.Entities.JournalContent.FinalPerformanceAccounting;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories.FinalPerformanceAccounting
{
    public class PerformanceAccountingColumnsRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IPerformanceAccountingColumnsRepository
    {
        public async Task<PerformanceAccountingColumn?> CreateAsync(PerformanceAccountingColumn column)
        {
            if (column == null) return null;
            if (!await CertificationTypeExists(column.CertificationTypeId)) return null;
            if (column.SubjectId != null && !await SubjectExists((int)column.SubjectId)) return null;
            if (!await PageExists(column.PageId)) return null;

            var created = await _dbContext.PerformanceAccountingColumns.AddAsync(column);

            await _dbContext.FinalPerformanceAccounting
                .Where(r => r.PageId == column.PageId)
                .ForEachAsync(r => r.PerformanceAccountingGrades.Add(new PerformanceAccountingGrade
                {
                    PerformanceAccountingColumn = created.Entity,
                    PerformanceAccountingRecord = r
                }));

            await _dbContext.SaveChangesAsync();

            return await _dbContext.PerformanceAccountingColumns
                .Include(c => c.CertificationType)
                .Include(c => c.Subject)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == created.Entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var column = await _dbContext.PerformanceAccountingColumns.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (column == null) return false;
            //if (!await CheckForDelete(column)) return false;

            await _dbContext.PerformanceAccountingGrades.Where(g => g.PerformanceAccountingColumnId == id).ExecuteDeleteAsync();

            var deletedRows = await _dbContext.PerformanceAccountingColumns.Where(c => c.Id == id).ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CertificationType>?> GetByPageIdGroupByCertificationTypes(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.CertificationTypes
                .Include(ct => ct.PerformanceAccountingColumns)
                .ThenInclude(c => c.Subject)
                .AsNoTracking()
                .Select(ct => new CertificationType
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    PerformanceAccountingColumns = ct.PerformanceAccountingColumns.Where(c => c.PageId == pageId).ToList()
                }).ToListAsync();
        }

        public async Task<List<PerformanceAccountingColumn>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.PerformanceAccountingColumns
                .Include(c => c.CertificationType)
                .Include(c => c.Subject)
                .AsNoTracking()
                .Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<PerformanceAccountingColumn?> UpdateAsync(int id, PerformanceAccountingColumn column)
        {
            if (column == null) return null;
            //if (!await CertificationTypeExists(column.CertificationTypeId)) return null;
            if (column.SubjectId != null && !await SubjectExists((int)column.SubjectId)) return null;

            var toUpdate = await _dbContext.PerformanceAccountingColumns
                .Include(c => c.CertificationType)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (toUpdate == null) return null;

            //toUpdate.CertificationTypeId = column.CertificationTypeId;
            toUpdate.SubjectId = column.SubjectId;

            await _dbContext.SaveChangesAsync();
            return await _dbContext.PerformanceAccountingColumns.Include(c => c.Subject).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.FinalPerformanceAccounting);

        private async Task<bool> CertificationTypeExists(int id) =>
            await _dbContext.CertificationTypes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> SubjectExists(int id) =>
            await _dbContext.Subjects.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> CheckForDelete(PerformanceAccountingColumn? column)
        {
            if (column == null) return false;

            var certificationType = await _dbContext.CertificationTypes
                .Include(c => c.PerformanceAccountingColumns)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == column.CertificationTypeId);

            if (certificationType == null) return false;

            return !((certificationType.Id == 1 || certificationType.Id == 2) && (certificationType.PerformanceAccountingColumns.Where(c => c.PageId == column.PageId).Count() <= 6));
        }
    }
}
