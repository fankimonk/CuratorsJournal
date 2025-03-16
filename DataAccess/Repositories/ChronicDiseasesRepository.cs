using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ChronicDiseasesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IChronicDiseasesRepository
    {
        public async Task<ChronicDisease?> CreateAsync(ChronicDisease disease)
        {
            if (disease == null) return null;

            var created = await _dbContext.ChronicDiseases.AddAsync(disease);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.ChronicDiseases.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ChronicDisease>> GetAllAsync()
        {
            return await _dbContext.ChronicDiseases.AsNoTracking().ToListAsync();
        }

        public async Task<ChronicDisease?> UpdateAsync(int id, ChronicDisease disease)
        {
            if (disease == null) return null;

            var diseaseToUpdate = await _dbContext.ChronicDiseases.FirstOrDefaultAsync(p => p.Id == id);
            if (diseaseToUpdate == null) return null;

            diseaseToUpdate.Name = disease.Name;

            await _dbContext.SaveChangesAsync();
            return diseaseToUpdate;
        }
    }
}
