using DataAccess.Interfaces;
using Domain.Entities.JournalContent.Pages.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class IdeologicalAndEducationalWorkPageAttributesRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext),
        IIdeologicalAndEducationalWorkPageAttributesRepository
    {
        public async Task<CuratorsIdeologicalAndEducationalWorkPageAttributes?> GetByPageId(int pageId)
        {
            return await _dbContext.CuratorsIdeologicalAndEducationalWorkPageAttributes.AsNoTracking().FirstOrDefaultAsync(h => h.PageId == pageId);
        }

        public async Task<CuratorsIdeologicalAndEducationalWorkPageAttributes?> UpdateAttributes(int id, int? month, int? year)
        {
            var attributes = await _dbContext.CuratorsIdeologicalAndEducationalWorkPageAttributes
                .FirstOrDefaultAsync(a => a.Id == id);
            if (attributes == null) return null;

            attributes.Month = month;
            attributes.Year = year;
            await _dbContext.SaveChangesAsync();

            return await _dbContext.CuratorsIdeologicalAndEducationalWorkPageAttributes.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
