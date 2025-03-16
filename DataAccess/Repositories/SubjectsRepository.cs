using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SubjectsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), ISubjectsRepository
    {
        public async Task<Subject?> CreateAsync(Subject subject)
        {
            if (subject == null) return null;

            var created = await _dbContext.Subjects.AddAsync(subject);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Subjects.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            return await _dbContext.Subjects.AsNoTracking().ToListAsync();
        }

        public async Task<Subject?> UpdateAsync(int id, Subject subject)
        {
            if (subject == null) return null;

            var subjectToUpdate = await _dbContext.Subjects.FirstOrDefaultAsync(p => p.Id == id);
            if (subjectToUpdate == null) return null;

            subjectToUpdate.Name = subject.Name;
            subjectToUpdate.AbbreviatedName = subject.AbbreviatedName;

            await _dbContext.SaveChangesAsync();
            return subjectToUpdate;
        }
    }
}
