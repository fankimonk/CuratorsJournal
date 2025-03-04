using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class TraditionsRepository(CuratorsJournalDBContext dBContext) : PageRepositoryBase(dBContext), ITraditionsRepository
    {
        public async Task<Tradition?> CreateAsync(Tradition tradition)
        {
            if (tradition == null) return null;
            if (!await PageExists(tradition.PageId)) return null;

            var createdTradition = await _dbContext.Traditions.AddAsync(tradition);

            await _dbContext.SaveChangesAsync();

            return createdTradition.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Traditions.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Tradition>?> GetByPageIdAsync(int pageId)
        {
            if (!await PageExists(pageId)) return null;
            return await _dbContext.Traditions.AsNoTracking().Where(c => c.PageId == pageId).ToListAsync();
        }

        public async Task<Tradition?> UpdateAsync(int id, Tradition tradition)
        {
            if (tradition == null) return null;

            var traditionToUpdate = await _dbContext.Traditions.FirstOrDefaultAsync(p => p.Id == id);
            if (traditionToUpdate == null) return null;

            traditionToUpdate.Name = tradition.Name;
            traditionToUpdate.ParticipationForm = tradition.ParticipationForm;
            traditionToUpdate.Note = tradition.Note;
            traditionToUpdate.Day = tradition.Day;
            traditionToUpdate.Month = tradition.Month;

            await _dbContext.SaveChangesAsync();
            return traditionToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.TraditionsPage);
    }
}
