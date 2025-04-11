using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class DepartmentsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IDepartmentsRepository
    {
        public async Task<Department?> CreateAsync(Department department)
        {
            if (department == null) return null;
            if (!await WorkerExists(department.HeadId)) return null;
            if (!await DeaneryExists(department.DeaneryId)) return null;

            var created = await _dbContext.Departments.AddAsync(department);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Departments.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Department>?> GetAllAsync(int userId)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var departments = _dbContext.Departments.AsNoTracking();

            if (user.RoleId == 1 || user.RoleId == 5) return await departments.ToListAsync();
            if (user.RoleId == 2 || user.RoleId == 3 || user.RoleId == 4)
            {
                departments = departments.Include(d => d!.Deanery);
                if (user.RoleId == 2) return await departments.Where(j => j.Deanery!.DeanId == user.WorkerId).ToListAsync();
                else if (user.RoleId == 3) return await departments.Where(j => j.Deanery!.DeputyDeanId == user.WorkerId).ToListAsync();
                else return await departments.Where(j => j.HeadId == user.WorkerId).ToListAsync();
            }
            if (user.RoleId == 6)
            {
                var teacher = await _dbContext.Teachers.Include(t => t.Department).AsNoTracking().FirstOrDefaultAsync(t => t.WorkerId == user.WorkerId);
                if (teacher == null) return null;
                return await departments.Where(j => j.Id == teacher.DepartmentId).ToListAsync();
            }

            return null;
        }

        public async Task<Department?> UpdateAsync(int id, Department department)
        {
            if (department == null) return null;
            if (!await WorkerExists(department.HeadId)) return null;
            if (!await DeaneryExists(department.DeaneryId)) return null;

            var departmentToUpdate = await _dbContext.Departments.FirstOrDefaultAsync(p => p.Id == id);
            if (departmentToUpdate == null) return null;

            departmentToUpdate.Name = department.Name;
            departmentToUpdate.AbbreviatedName = department.AbbreviatedName;
            departmentToUpdate.HeadId = department.HeadId;
            departmentToUpdate.DeaneryId = department.DeaneryId;

            await _dbContext.SaveChangesAsync();
            return departmentToUpdate;
        }

        private async Task<bool> WorkerExists(int id) =>
            await _dbContext.Workers.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id) != null;

        private async Task<bool> DeaneryExists(int id) =>
            await _dbContext.Deaneries.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id) != null;
    }
}
