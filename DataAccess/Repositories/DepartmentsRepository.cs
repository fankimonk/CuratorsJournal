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
            if (!await HeadExists(department.HeadId)) return null;
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

        public async Task<List<Department>> GetAllAsync()
        {
            return await _dbContext.Departments.AsNoTracking().ToListAsync();
        }

        public async Task<Department?> UpdateAsync(int id, Department department)
        {
            if (department == null) return null;
            if (!await HeadExists(department.HeadId)) return null;
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

        private async Task<bool> HeadExists(int id) =>
            await _dbContext.HeadsOfDepartments.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id) != null;

        private async Task<bool> DeaneryExists(int id) =>
            await _dbContext.Deaneries.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id) != null;
    }
}
