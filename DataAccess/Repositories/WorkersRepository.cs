using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class WorkersRepository(CuratorsJournalDBContext dBContext) : RepositoryBase(dBContext), IWorkersRepository
    {
        public async Task<List<Worker>> GetAllAsync()
        {
            return await _dbContext.Workers.Include(w => w.Position).AsNoTracking().ToListAsync();
        }

        public async Task<Worker?> GetById(int id)
        {
            return await _dbContext.Workers.Include(w => w.Position).AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
