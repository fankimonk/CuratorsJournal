using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AcademicYearsRepository(CuratorsJournalDBContext dBContext) : RepositoryBase(dBContext), IAcademicYearsRepository
    {
        public async Task<List<AcademicYear>> GetAll(int? yearSince = null)
        {
            var academicYears = _dbContext.AcademicYears.AsNoTracking();

            if (yearSince != null)
                return await academicYears.Where(a => a.StartYear >= yearSince).ToListAsync();

            return await academicYears.ToListAsync();
        }
    }
}
