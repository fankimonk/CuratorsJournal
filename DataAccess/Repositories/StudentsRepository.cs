using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class StudentsRepository(CuratorsJournalDBContext dbContext) : RepositoryBase(dbContext), IStudentsRepository
    {
        public async Task<Student?> CreateAsync(Student student)
        {
            if (student == null) return null;
            if (!await GroupExists(student.GroupId)) return null;
            if (student.UserId != null && !await UserExists((int)student.UserId)) return null;

            var created = await _dbContext.Students.AddAsync(student);

            await _dbContext.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedRows = await _dbContext.Students.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedRows < 1) return false;

            await _dbContext.SaveChangesAsync();
            return true;
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

        public async Task<Student?> UpdateAsync(int id, Student student)
        {
            if (student == null) return null;
            if (!await GroupExists(student.GroupId)) return null;
            if (student.UserId != null && !await UserExists((int)student.UserId)) return null;

            var studentToUpdate = await _dbContext.Students.FirstOrDefaultAsync(p => p.Id == id);
            if (studentToUpdate == null) return null;

            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.MiddleName = student.MiddleName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.PhoneNumber = student.PhoneNumber;
            studentToUpdate.GroupId = student.GroupId;
            studentToUpdate.UserId = student.UserId;

            await _dbContext.SaveChangesAsync();
            return studentToUpdate;
        }

        private async Task<bool> GroupExists(int id) =>
            await _dbContext.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id) != null;

        private async Task<bool> JournalExists(int id) =>
            await _dbContext.Journals.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;

        private async Task<bool> UserExists(int id) =>
            await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id) != null;
    }
}
