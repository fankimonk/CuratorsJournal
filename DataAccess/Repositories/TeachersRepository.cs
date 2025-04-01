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

        public async Task<List<Teacher>> GetAllAsync()
        {
            return await _dbContext.Teachers.Include(t => t.Worker).AsNoTracking().ToListAsync();
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
