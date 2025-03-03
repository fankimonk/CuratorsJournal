using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SpecialtiesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ISpecialtiesRepository
    {
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
