using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CuratorsRepository : ICuratorsRepository
    {
        private readonly CuratorsJournalDBContext _dbContext;

        public CuratorsRepository(CuratorsJournalDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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
