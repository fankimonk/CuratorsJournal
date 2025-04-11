using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SpecialtiesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ISpecialtiesRepository
    {
        public async Task<List<Specialty>?> GetAllAsync(int userId)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var specialties = _dbContext.Specialties.AsNoTracking();

            if (user.RoleId == 1 || user.RoleId == 5) return await specialties.ToListAsync();
            if (user.RoleId == 2 || user.RoleId == 3 || user.RoleId == 4)
            {
                specialties = specialties.Include(s => s!.Department).ThenInclude(d => d!.Deanery);
                if (user.RoleId == 2) return await specialties.Where(j => j.Department!.Deanery!.DeanId == user.WorkerId).ToListAsync();
                else if (user.RoleId == 3) return await specialties.Where(j => j.Department!.Deanery!.DeputyDeanId == user.WorkerId).ToListAsync();
                else return await specialties.Where(j => j.Department!.HeadId == user.WorkerId).ToListAsync();
            }
            if (user.RoleId == 6)
            {
                var teacher = await _dbContext.Teachers.Include(t => t.Department).AsNoTracking().FirstOrDefaultAsync(t => t.WorkerId == user.WorkerId);
                if (teacher == null) return null;
                return await specialties.Where(j => j.DepartmentId == teacher.DepartmentId).ToListAsync();
            }

            return null;
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
