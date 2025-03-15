using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class FacultiesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IFacultiesRepository
    {
        public async Task<Faculty?> CreateAsync(Faculty faculty)
        {
            if (faculty == null) return null;

            var createdFaculty = await _dbContext.Faculties.AddAsync(faculty);

            await _dbContext.SaveChangesAsync();

            return createdFaculty.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Faculties.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Faculty>> GetAllAsync()
        {
            return await _dbContext.Faculties.AsNoTracking().ToListAsync();
        }

        public async Task<Faculty?> UpdateAsync(int id, Faculty faculty)
        {
            if (faculty == null) return null;

            var facultyToUpdate = await _dbContext.Faculties.FirstOrDefaultAsync(p => p.Id == id);
            if (facultyToUpdate == null) return null;

            facultyToUpdate.Name = faculty.Name;
            facultyToUpdate.AbbreviatedName = faculty.AbbreviatedName;

            await _dbContext.SaveChangesAsync();
            return facultyToUpdate;
        }
    }
}
