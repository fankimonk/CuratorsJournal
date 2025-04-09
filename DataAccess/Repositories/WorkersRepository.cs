using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class WorkersRepository(CuratorsJournalDBContext dBContext) : RepositoryBase(dBContext), IWorkersRepository
    {
        public async Task<List<Worker>> GetAllAsync(int? positionId)
        {
            var workers = _dbContext.Workers.Include(w => w.Position).AsNoTracking();
            if (positionId != null) workers = workers.Where(w => w.PositionId == positionId);
            return await workers.ToListAsync();
        }

        public async Task<Worker?> GetById(int id)
        {
            return await _dbContext.Workers.Include(w => w.Position).AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Worker?> CreateAsync(Worker worker)
        {
            if (worker == null) return null;
            if (!await PositionExists(worker.PositionId)) return null;

            var created = await _dbContext.Workers.AddAsync(worker);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Workers.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Worker?> UpdateAsync(int id, Worker worker)
        {
            if (worker == null) return null;
            if (!await PositionExists(worker.PositionId)) return null;

            var workerToUpdate = await _dbContext.Workers.FirstOrDefaultAsync(p => p.Id == id);
            if (workerToUpdate == null) return null;

            workerToUpdate.FirstName = worker.FirstName;
            workerToUpdate.MiddleName = worker.MiddleName;
            workerToUpdate.LastName = worker.LastName;
            workerToUpdate.PositionId = worker.PositionId;

            await _dbContext.SaveChangesAsync();
            return workerToUpdate;
        }

        private async Task<bool> PositionExists(int id) =>
            await _dbContext.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id) != null;
    }
}
