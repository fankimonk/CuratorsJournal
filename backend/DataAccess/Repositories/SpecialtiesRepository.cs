using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SpecialtiesRepository : ISpecialtiesRepository
    {
        private readonly CuratorsJournalDBContext _dbContext;

        public SpecialtiesRepository(CuratorsJournalDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Specialty>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Specialty?> GetByIdAsync(int id)
        {
            return await _dbContext.Specialties.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
