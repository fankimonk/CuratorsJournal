using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SpecialtiesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ISpecialtiesRepository
    {
        public async Task<List<Specialty>> GetAllAsync()
        {
            return await _dbContext.Specialties.AsNoTracking().ToListAsync();
        }

        public async Task<Specialty?> GetByIdAsync(int id)
        {
            return await _dbContext.Specialties.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Specialty?> CreateAsync(Specialty specialty)
        {
            if (specialty == null) return null;
            if (!await DepartmentExists(specialty.DepartmentId)) return null;

            var created = await _dbContext.Specialties.AddAsync(specialty);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Specialties.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Specialty?> UpdateAsync(int id, Specialty specialty)
        {
            if (specialty == null) return null;
            if (!await DepartmentExists(specialty.DepartmentId)) return null;

            var specialtyToUpdate = await _dbContext.Specialties.FirstOrDefaultAsync(p => p.Id == id);
            if (specialtyToUpdate == null) return null;

            specialtyToUpdate.Name = specialty.Name;
            specialtyToUpdate.AbbreviatedName = specialty.AbbreviatedName;
            specialtyToUpdate.DepartmentId = specialty.DepartmentId;

            await _dbContext.SaveChangesAsync();
            return specialtyToUpdate;
        }

        private async Task<bool> DepartmentExists(int id) =>
            await _dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id) != null;
    }
}
