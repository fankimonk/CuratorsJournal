using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class DeaneriesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IDeaneriesRepository
    {
        public async Task<Deanery?> CreateAsync(Deanery deanery)
        {
            if (deanery == null) return null;
            if (!await FacultyExists(deanery.FacultyId)) return null;
            if (!await WorkerExists(deanery.DeanId)) return null;
            if (!await WorkerExists(deanery.DeputyDeanId)) return null;

            var created = await _dbContext.Deaneries.AddAsync(deanery);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Deaneries.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Deanery>?> GetAllAsync(int userId)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var deaneries = _dbContext.Deaneries.Include(d => d.Faculty).AsNoTracking();

            if (user.RoleId == 1 || user.RoleId == 5) return await deaneries.ToListAsync();
            if (user.RoleId == 2 || user.RoleId == 3 || user.RoleId == 4)
            {
                if (user.RoleId == 2) return await deaneries.Where(j => j.DeanId == user.WorkerId).ToListAsync();
                else if (user.RoleId == 3) return await deaneries.Where(j => j.DeputyDeanId == user.WorkerId).ToListAsync();
                else return await deaneries.Include(d => d.Departments).Where(j => j.Departments.Any(d => d.HeadId == user.WorkerId)).ToListAsync();
            }
            if (user.RoleId == 6)
            {
                var teacher = await _dbContext.Teachers.Include(t => t.Department).AsNoTracking().FirstOrDefaultAsync(t => t.WorkerId == user.WorkerId);
                if (teacher == null) return null;
                return await deaneries.Where(j => j.Id == teacher.Department!.DeaneryId).ToListAsync();
            }

            return null;
        }

        public async Task<Deanery?> UpdateAsync(int id, Deanery deanery)
        {
            if (deanery == null) return null;
            if (!await FacultyExists(deanery.FacultyId)) return null;
            if (!await WorkerExists(deanery.DeanId)) return null;
            if (!await WorkerExists(deanery.DeputyDeanId)) return null;

            var deaneryToUpdate = await _dbContext.Deaneries.FirstOrDefaultAsync(p => p.Id == id);
            if (deaneryToUpdate == null) return null;

            deaneryToUpdate.FacultyId = deanery.FacultyId;
            deaneryToUpdate.DeanId = deanery.DeanId;
            deaneryToUpdate.DeputyDeanId = deanery.DeputyDeanId;

            await _dbContext.SaveChangesAsync();
            return deaneryToUpdate;
        }

        private async Task<bool> FacultyExists(int id) =>
            await _dbContext.Faculties.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id) != null;

        private async Task<bool> WorkerExists(int id) =>
            await _dbContext.Workers.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id) != null;
    }
}
