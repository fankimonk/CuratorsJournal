using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AcademicYearsRepository(CuratorsJournalDBContext dBContext) : RepositoryBase(dBContext), IAcademicYearsRepository
    {
        public async Task<List<AcademicYear>> GetAllAsync(int? yearSince = null)
        {
            var academicYears = _dbContext.AcademicYears.AsNoTracking();

            if (yearSince != null)
                return await academicYears.Where(a => a.StartYear >= yearSince).ToListAsync();

            return await academicYears.ToListAsync();
        }

        public async Task<AcademicYear?> CreateAsync(AcademicYear academicYear)
        {
            if (academicYear == null) return null;

            var academicYears = _dbContext.AcademicYears;
            if (academicYears.Any(ay => ay.StartYear == academicYear.StartYear || ay.EndYear == academicYear.EndYear)) return null;
            var created = await academicYears.AddAsync(academicYear);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.AcademicYears.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<AcademicYear?> UpdateAsync(int id, AcademicYear academicYear)
        {
            if (academicYear == null) return null;

            var academicYearToUpdate = await _dbContext.AcademicYears.FirstOrDefaultAsync(p => p.Id == id);
            if (academicYearToUpdate == null) return null;

            academicYearToUpdate.StartYear = academicYear.StartYear;
            academicYearToUpdate.EndYear = academicYear.EndYear;

            await _dbContext.SaveChangesAsync();
            return academicYearToUpdate;
        }
    }
}
