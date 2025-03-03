using DataAccess.Interfaces;
using Domain.Entities.JournalContent.Pages;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PagesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IPagesRepository
    {
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Page?> GetById(int id)
        {
            return await _dbContext.Pages.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Page>> GetByJournalId(int journalId)
        {
            return await _dbContext.Pages.AsNoTracking().Include(p => p.PageType).Where(p => p.JournalId == journalId).ToListAsync();
        }
    }
}
