using DataAccess.Interfaces.PageRepositories;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.PageRepositories
{
    public class PsychologicalAndPedagogicalCharacteristicsRepository(CuratorsJournalDBContext dBContext)
        : PageRepositoryBase(dBContext), IPsychologicalAndPedagogicalCharacteristicsRepository
    {
        public async Task<PsychologicalAndPedagogicalCharacteristics?> CreateAsync(
            PsychologicalAndPedagogicalCharacteristics characteristics)
        {
            if (characteristics == null) return null;
            if (!await PageExists(characteristics.PageId)) return null;

            var createdCharacteristics = await _dbContext.PsychologicalAndPedagogicalCharacteristics.AddAsync(characteristics);

            await _dbContext.SaveChangesAsync();

            return createdCharacteristics.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.PsychologicalAndPedagogicalCharacteristics.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PsychologicalAndPedagogicalCharacteristics?> GetByPageIdAsync(int id)
        {
            var pageExists = await PageExists(id);
            if (!pageExists) return null;
            return await _dbContext.PsychologicalAndPedagogicalCharacteristics
                .AsNoTracking().FirstOrDefaultAsync(c => c.PageId == id);
        }

        public async Task<PsychologicalAndPedagogicalCharacteristics?> UpdateAsync(int id, 
            PsychologicalAndPedagogicalCharacteristics characteristics)
        {
            if (characteristics == null) return null;
            if (characteristics.WorkerId != null && !await WorkerExists((int)characteristics.WorkerId)) return null;

            var characteristicsToUpdate = await _dbContext.PsychologicalAndPedagogicalCharacteristics
                .FirstOrDefaultAsync(p => p.Id == id);
            if (characteristicsToUpdate == null) return null;

            characteristicsToUpdate.Content = characteristics.Content;
            characteristicsToUpdate.Date = characteristics.Date;
            characteristicsToUpdate.WorkerId = characteristics.WorkerId;

            await _dbContext.SaveChangesAsync();
            return characteristicsToUpdate;
        }

        public async Task<bool> PageExists(int id) => await PageExists(id, PageTypes.PsychologicalAndPedagogicalCharacteristics);

        private async Task<bool> WorkerExists(int id) =>
            await _dbContext.Workers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) != null;
    }
}
