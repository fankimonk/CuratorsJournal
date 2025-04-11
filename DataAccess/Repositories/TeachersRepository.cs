using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class TeachersRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ITeachersRepository
    {
        public async Task<Teacher?> CreateAsync(Teacher teacher)
        {
            if (teacher == null) return null;
            if (!await DepartmentExists(teacher.DepartmentId)) return null;

            var createdFaculty = await _dbContext.Teachers.AddAsync(teacher);

            await _dbContext.SaveChangesAsync();

            return createdFaculty.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Teachers.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Teacher>?> GetAllAsync(int userId)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var teachers = _dbContext.Teachers.Include(t => t.Worker).AsNoTracking();

            if (user.RoleId == 1 || user.RoleId == 5) return await teachers.ToListAsync();
            if (user.RoleId == 2 || user.RoleId == 3 || user.RoleId == 4)
            {
                teachers = teachers.Include(s => s!.Department).ThenInclude(d => d!.Deanery);
                if (user.RoleId == 2) return await teachers.Where(j => j.Department!.Deanery!.DeanId == user.WorkerId).ToListAsync();
                else if (user.RoleId == 3) return await teachers.Where(j => j.Department!.Deanery!.DeputyDeanId == user.WorkerId).ToListAsync();
                else return await teachers.Where(j => j.Department!.HeadId == user.WorkerId).ToListAsync();
            }
            if (user.RoleId == 6)
            {
                var teacher = await _dbContext.Teachers.Include(t => t.Department).AsNoTracking().FirstOrDefaultAsync(t => t.WorkerId == user.WorkerId);
                if (teacher == null) return null;
                return await teachers.Where(j => j.DepartmentId == teacher.DepartmentId).ToListAsync();
            }

            return null;
        }

        public async Task<Teacher?> GetByIdAsync(int id)
        {
            return await _dbContext.Teachers.Include(t => t.Worker).AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Teacher?> UpdateAsync(int id, Teacher teacher)
        {
            if (teacher == null) return null;
            if (!await DepartmentExists(teacher.DepartmentId)) return null;

            var toUpdate = await _dbContext.Teachers.FirstOrDefaultAsync(p => p.Id == id);
            if (toUpdate == null) return null;

            toUpdate.DepartmentId = teacher.DepartmentId;

            await _dbContext.SaveChangesAsync();
            return toUpdate;
        }

        private async Task<bool> DepartmentExists(int id) =>
            await _dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id) != null;
    }
}
