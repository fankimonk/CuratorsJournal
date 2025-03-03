using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public abstract class PageRepositoryBase(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext)
    {
        protected async Task<bool> PageExists(int id, PageTypes pageType)
        {
            return await _dbContext.Pages.AsNoTracking()
                .Where(p => p.PageTypeId == (int)pageType)
                .FirstOrDefaultAsync(p => p.Id == id) != null;
        }
    }
}
