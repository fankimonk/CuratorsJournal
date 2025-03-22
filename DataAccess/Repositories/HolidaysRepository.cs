using DataAccess.Interfaces;
using Domain.Entities.JournalContent.Holidays;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class HolidaysRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IHolidaysRepository
    {
        public async Task<Holiday?> CreateAsync(Holiday holiday)
        {
            if (holiday == null) return null;
            if (!await TypeExists(holiday.TypeId)) return null;

            var created = await _dbContext.Holidays.AddAsync(holiday);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<HolidayType?> CreateTypeAsync(HolidayType type)
        {
            if (type == null) return null;

            var created = await _dbContext.HolidayTypes.AddAsync(type);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Holidays.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTypeAsync(int id)
        {
            var deletedRows = await _dbContext.HolidayTypes.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<HolidayType>> GetGroupedByTypes()
        {
            return await _dbContext.HolidayTypes.AsNoTracking().Include(ht => ht.Holidays).ToListAsync();
        }

        public async Task<Holiday?> UpdateAsync(int id, Holiday holiday)
        {
            if (holiday == null) return null;

            var academicYearToUpdate = await _dbContext.Holidays.FirstOrDefaultAsync(p => p.Id == id);
            if (academicYearToUpdate == null) return null;

            academicYearToUpdate.Day = holiday.Day;
            academicYearToUpdate.Month = holiday.Month;
            academicYearToUpdate.RelativeDate = holiday.RelativeDate;
            academicYearToUpdate.Name = holiday.Name;

            await _dbContext.SaveChangesAsync();
            return academicYearToUpdate;
        }

        public async Task<HolidayType?> UpdateTypeAsync(int id, HolidayType type)
        {
            if (type == null) return null;

            var academicYearToUpdate = await _dbContext.HolidayTypes.FirstOrDefaultAsync(p => p.Id == id);
            if (academicYearToUpdate == null) return null;

            academicYearToUpdate.Name = type.Name;

            await _dbContext.SaveChangesAsync();
            return academicYearToUpdate;
        }

        private async Task<bool> TypeExists(int id) =>
            await _dbContext.HolidayTypes.AsNoTracking().FirstOrDefaultAsync(ht => ht.Id == id) != null;
    }
}
