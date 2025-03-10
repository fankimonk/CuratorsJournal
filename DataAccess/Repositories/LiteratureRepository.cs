using DataAccess.Interfaces;
using Domain.Entities.JournalContent.Literature;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class LiteratureRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ILiteratureRepository
    {
        public async Task<LiteratureListRecord?> CreateAsync(LiteratureListRecord literature)
        {
            if (literature == null) return null;

            var createdLiterature = await _dbContext.LiteratureList.AddAsync(literature);

            await _dbContext.SaveChangesAsync();

            return createdLiterature.Entity;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LiteratureListRecord>> GetAllAsync()
        {
            return await _dbContext.LiteratureList.AsNoTracking().ToListAsync();
        }

        public Task<LiteratureListRecord?> UpdateAsync(int id, LiteratureListRecord literature)
        {
            throw new NotImplementedException();
        }
    }
}
