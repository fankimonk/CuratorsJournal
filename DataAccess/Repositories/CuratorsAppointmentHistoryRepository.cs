using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CuratorsAppointmentHistoryRepository : ICuratorsAppointmentHistoryRepository
    {
        private readonly CuratorsJournalDBContext _dbContext;

        public CuratorsAppointmentHistoryRepository(CuratorsJournalDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CuratorsAppointmentHistoryRecord?> CreateAsync(CuratorsAppointmentHistoryRecord record)
        {
            if (record == null) return null;

            if (!await CuratorExists(record.CuratorId)) return null;
            if (!await GroupExists(record.GroupId)) return null;

            var createdRecord = await _dbContext.CuratorsAppointmentHistory.AddAsync(record);

            await _dbContext.SaveChangesAsync();

            return createdRecord.Entity;
        }

        public Task<List<CuratorsAppointmentHistoryRecord>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<bool> CuratorExists(int id) =>
            await _dbContext.Curators.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;

        private async Task<bool> GroupExists(int id) =>
            await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
