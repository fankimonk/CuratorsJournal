using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class JournalsRepository : IJournalsRepository
    {
        private readonly CuratorsJournalDBContext _dbContext;

        public JournalsRepository(CuratorsJournalDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Journal>> GetAllAsync()
        {
            return await _dbContext.Journals.AsNoTracking()
                .Include(j => j.Group).ThenInclude(g => g!.Curator).ThenInclude(c => c!.Teacher).ThenInclude(t => t!.Worker)
                .ToListAsync();
        }

        public async Task<Journal?> GetJournalWithTitlePageData(int journalId)
        {
            return await _dbContext.Journals.AsNoTracking()
                .Include(j => j.Group).ThenInclude(g => g!.Curator).ThenInclude(c => c!.Teacher).ThenInclude(t => t!.Worker)
                .Include(j => j.Group).ThenInclude(g => g!.Specialty).ThenInclude(s => s!.Department).ThenInclude(d => d!.Deanery).ThenInclude(d => d!.Faculty)
                .FirstOrDefaultAsync(j => j.Id == journalId);
        }

        public async Task<Journal?> CreateAsync(Journal journal)
        {
            if (journal == null) return null;

            var createdJournal = await _dbContext.Journals.AddAsync(journal);

            if (createdJournal == null) return null;

            await _dbContext.SaveChangesAsync();

            return createdJournal.Entity;
        }
    }
}
