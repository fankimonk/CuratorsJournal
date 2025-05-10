using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class JournalsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IJournalsRepository
    {
        public async Task<List<Journal>?> GetAllAsync(int userId)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var journals = _dbContext.Journals.Include(j => j.Group)
                .ThenInclude(c => c!.Curator).ThenInclude(t => t!.Worker).AsNoTracking();

            if (user.RoleId == 1 || user.RoleId == 5) return await journals.ToListAsync();
            if (user.RoleId == 2 || user.RoleId == 3 || user.RoleId == 4)
            {
                journals = journals
                        .Include(j => j.Group)
                        .ThenInclude(g => g!.Specialty)
                        .ThenInclude(s => s!.Department)
                        .ThenInclude(d => d!.Deanery);
                if (user.RoleId == 2) return await journals.Where(j => j.Group!.Specialty!.Department!.Deanery!.DeanId == user.WorkerId).ToListAsync();
                else if (user.RoleId == 3) return await journals.Where(j => j.Group!.Specialty!.Department!.Deanery!.DeputyDeanId == user.WorkerId).ToListAsync();
                else return await journals.Where(j => j.Group!.Specialty!.Department!.HeadId == user.WorkerId).ToListAsync();
            }
            if (user.RoleId == 6)
            {
                var teacher = await _dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.WorkerId == user.WorkerId);
                if (teacher == null) return null;
                return await journals.Where(j => j.Group!.CuratorId == teacher.Id).ToListAsync();
            }

            return null;
        }

        public async Task<bool> VerifyAccess(int userId, int journalId)
        {
            var journals = await GetAllAsync(userId);
            return journals != null && journals.Any(j => j.Id == journalId);
        }

        public async Task<Journal?> GetByIdAsync(int journalId)
        {
            return await _dbContext.Journals.AsNoTracking()
                .Include(j => j.Group).ThenInclude(g => g!.Curator).ThenInclude(t => t!.Worker)
                .Include(j => j.Group).ThenInclude(g => g!.Specialty).ThenInclude(s => s!.Department).ThenInclude(d => d!.Deanery).ThenInclude(d => d!.Faculty)
                .FirstOrDefaultAsync(j => j.Id == journalId);
        }

        public async Task<Journal?> CreateAsync(Journal journal)
        {
            if (journal == null) return null;
            if (!await GroupExists(journal.GroupId)) return null;

            var createdJournal = await _dbContext.Journals.AddAsync(journal);

            await _dbContext.SaveChangesAsync();

            return createdJournal.Entity;
        }

        private async Task<bool> GroupExists(int id) =>
            await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id) != null;

        public async Task<bool> Exists(int id)
        {
            return await _dbContext.Journals.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;
        }
    }
}
