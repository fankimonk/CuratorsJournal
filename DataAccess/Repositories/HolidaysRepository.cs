using DataAccess.Interfaces;
using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class HolidaysRepository : IHolidaysRepository
    {
        private readonly CuratorsJournalDBContext _dbContext;

        public HolidaysRepository(CuratorsJournalDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Holiday?> CreateAsync(Holiday holiday)
        {
            throw new NotImplementedException();
        }

        public Task<HolidayType?> CreateTypeAsync(HolidayType type)
        {
            throw new NotImplementedException();
        }

        public async Task<List<HolidayType>> GetGroupedByTypes()
        {
            return await _dbContext.HolidayTypes.AsNoTracking().Include(ht => ht.Holidays).ToListAsync();
        }

        public Task<Holiday?> UpdateAsync(int id, Holiday holiday)
        {
            throw new NotImplementedException();
        }

        public Task<HolidayType?> UpdateTypeAsync(int id, HolidayType type)
        {
            throw new NotImplementedException();
        }
    }
}
