using DataAccess.Interfaces;
using Domain.Entities;

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

        public Task<List<Student>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetByGroupIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Student?> UpdateAsync(int id, Student student)
        {
            throw new NotImplementedException();
        }
    }
}
