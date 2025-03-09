using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class StudentsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IStudentsRepository
    {
        public Task<Student?> CreateAsync(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.Students.AsNoTracking().ToListAsync();
        }

        public async Task<List<Student>?> GetByGroupIdAsync(int id)
        {
            if (!await GroupExists(id)) return null;
            return await _dbContext.Students.AsNoTracking().Where(s => s.GroupId == id).ToListAsync();
        }

        public async Task<List<Student>?> GetByJournalIdAsync(int id)
        {
            if (!await JournalExists(id)) return null;
            return await _dbContext.Students.Include(s => s.Group).ThenInclude(g => g!.Journal).AsNoTracking()
                .Where(s => s.Group!.Journal!.Id == id).ToListAsync();
        }

        public Task<Student?> UpdateAsync(int id, Student student)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> GroupExists(int id) =>
            await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id) != null;

        private async Task<bool> JournalExists(int id) =>
            await _dbContext.Journals.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;
    }
}
