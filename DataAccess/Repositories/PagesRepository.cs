using DataAccess.Interfaces;
using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PagesRepository : IPagesRepository
    {
        private readonly CuratorsJournalDBContext _dbContext;

        public PagesRepository(CuratorsJournalDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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
