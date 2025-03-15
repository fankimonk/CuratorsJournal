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

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.LiteratureList.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<LiteratureListRecord>> GetAllAsync()
        {
            return await _dbContext.LiteratureList.AsNoTracking().ToListAsync();
        }

        public async Task<LiteratureListRecord?> UpdateAsync(int id, LiteratureListRecord literature)
        {
            if (literature == null) return null;

            var literatureToUpdate = await _dbContext.LiteratureList.FirstOrDefaultAsync(p => p.Id == id);
            if (literatureToUpdate == null) return null;

            literatureToUpdate.Author = literature.Author;
            literatureToUpdate.Name = literature.Name;
            literatureToUpdate.BibliographicData = literature.BibliographicData;

            await _dbContext.SaveChangesAsync();
            return literatureToUpdate;
        }
    }
}
