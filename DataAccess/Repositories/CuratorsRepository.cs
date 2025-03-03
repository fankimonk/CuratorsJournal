using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CuratorsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ICuratorsRepository
    {
        public async Task<List<Curator>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Curator?> GetByIdAsync(int id)
        {
            return await _dbContext.Curators.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
