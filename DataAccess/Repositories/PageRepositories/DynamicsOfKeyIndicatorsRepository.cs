using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class DynamicsOfKeyIndicatorsRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), IDynamicsOfKeyIndicatorsRepository
    {
        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.DynamicsOfKeyIndicators.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<DynamicsOfKeyIndicatorsRecord>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.DynamicsOfKeyIndicators
                .Include(d => d.KeyIndicatorsByCourse).Include(d => d.KeyIndicator)
                .AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        //public async Task<KeyIndicatorByCourse?> AddValueAsync(KeyIndicatorByCourse value)
        //{
        //    if (value == null) return null;
        //    if (!await RecordExists(value.DynamicsRecordId)) return null;

        //    var createdValue = await _dbContext.KeyIndicatorsByCourse.AddAsync(value);

        //    await _dbContext.SaveChangesAsync();

        //    return createdValue.Entity;
        //}

        public async Task<KeyIndicatorByCourse?> UpdateValueAsync(int id, KeyIndicatorByCourse value)
        {
            if (value == null) return null;

            var valueToUpdate = await _dbContext.KeyIndicatorsByCourse.FirstOrDefaultAsync(p => p.Id == id);
            if (valueToUpdate == null) return null;

            valueToUpdate.Value = value.Value;

            await _dbContext.SaveChangesAsync();
            return valueToUpdate;
        }

        public async Task<DynamicsOfKeyIndicatorsRecord?> UpdateAsync(int id, DynamicsOfKeyIndicatorsRecord record)
        {
            if (record == null) return null;

            var recordToUpdate = await _dbContext.DynamicsOfKeyIndicators
                .Include(d => d.KeyIndicatorsByCourse).Include(d => d.KeyIndicator)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (recordToUpdate == null) return null;

            recordToUpdate.Note = record.Note;

            await _dbContext.SaveChangesAsync();
            return recordToUpdate;
        }

        public async Task<bool> AddCourseAsync(int pageId)
        {
            if (!await PageExists(pageId)) return false;

            var records = _dbContext.DynamicsOfKeyIndicators.Include(r => r.KeyIndicatorsByCourse).Where(r => r.PageId == pageId);
            var maxCourse = records.Select(r => r.KeyIndicatorsByCourse).SelectMany(z => z).Max(z => z.Course);

            await records.ForEachAsync(r => r.KeyIndicatorsByCourse.Add(new KeyIndicatorByCourse { Course = maxCourse + 1, Value = null }));
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCourseAsync(int pageId)
        {
            if (!await PageExists(pageId)) return false;

            var records = _dbContext.DynamicsOfKeyIndicators.Include(r => r.KeyIndicatorsByCourse).Where(r => r.PageId == pageId);
            var maxCourse = records.Select(r => r.KeyIndicatorsByCourse).SelectMany(z => z).Max(z => z.Course);
            if (maxCourse == 1) return false;

            await records.ForEachAsync(r => r.KeyIndicatorsByCourse.RemoveAll(z => z.Course == maxCourse));
            await _dbContext.SaveChangesAsync();

            return true;
        }

        //public async Task<bool> DeleteValueAsync(int id)
        //{
        //    var deletedRows = await _dbContext.KeyIndicatorsByCourse.Where(c => c.Id == id).ExecuteDeleteAsync();
        //    if (deletedRows < 1) return false;

        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.DynamicsOfKeyIndicators);

        private async Task<bool> RecordExists(int id)
            => await _dbContext.DynamicsOfKeyIndicators.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id) != null;
    }
}
