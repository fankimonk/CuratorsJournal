using DataAccess.Interfaces;
using Domain.Enums.Journal;
using Domain.Entities.JournalContent.Holidays;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class HolidaysRepository(CuratorsJournalDBContext dbContext) : PageRepositoryBase(dbContext), IHolidaysRepository
    {
        public async Task<Holiday?> CreateAsync(Holiday holiday)
        {
            if (holiday == null) return null;
            if (!await PageExists(holiday.PageId)) return null;
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

        public async Task<List<HolidayType>> GetAllTypesAsync()
        {
            return await _dbContext.HolidayTypes.AsNoTracking().ToListAsync();
        }

        public async Task<List<HolidayType>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.HolidayTypes.AsNoTracking()
                .Include(ht => ht.Holidays)
                .Select(ht => new HolidayType
                {
                    Id = ht.Id,
                    Name = ht.Name,
                    Holidays = ht.Holidays.Where(h => h.PageId == pageId).ToList()
                }).ToListAsync();
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.Holidays);

        public async Task<Holiday?> UpdateAsync(int id, Holiday holiday)
        {
            if (holiday == null) return null;

            var toUpdate = await _dbContext.Holidays.FirstOrDefaultAsync(p => p.Id == id);
            if (toUpdate == null) return null;

            toUpdate.Day = holiday.Day;
            toUpdate.Month = holiday.Month;
            toUpdate.RelativeDate = holiday.RelativeDate;
            toUpdate.Name = holiday.Name;
            toUpdate.IsRelativeDate = holiday.IsRelativeDate;

            await _dbContext.SaveChangesAsync();
            return toUpdate;
        }

        public async Task<HolidayType?> UpdateTypeAsync(int id, HolidayType type)
        {
            if (type == null) return null;

            var toUpdate = await _dbContext.HolidayTypes.FirstOrDefaultAsync(p => p.Id == id);
            if (toUpdate == null) return null;

            toUpdate.Name = type.Name;

            await _dbContext.SaveChangesAsync();
            return toUpdate;
        }

        private async Task<bool> TypeExists(int id) =>
            await _dbContext.HolidayTypes.AsNoTracking().FirstOrDefaultAsync(ht => ht.Id == id) != null;
    }
}
