using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CertificationTypesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ICertificationTypesRepository
    {
        public async Task<CertificationType?> CreateAsync(CertificationType type)
        {
            if (type == null) return null;

            var created = await _dbContext.CertificationTypes.AddAsync(type);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.CertificationTypes.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CertificationType>> GetAllAsync()
        {
            return await _dbContext.CertificationTypes.AsNoTracking().ToListAsync();
        }

        public async Task<CertificationType?> UpdateAsync(int id, CertificationType type)
        {
            if (type == null) return null;

            var typeToUpdate = await _dbContext.CertificationTypes.FirstOrDefaultAsync(p => p.Id == id);
            if (typeToUpdate == null) return null;

            typeToUpdate.Name = type.Name;

            await _dbContext.SaveChangesAsync();
            return typeToUpdate;
        }
    }
}
