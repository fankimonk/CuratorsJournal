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

        public async Task<List<Deanery>> GetAllAsync()
        {
            return await _dbContext.Deaneries.Include(d => d.Faculty).AsNoTracking().ToListAsync();
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
