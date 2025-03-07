using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IStudentsRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<List<Student>> GetByGroupIdAsync(int id);
        Task<Student?> CreateAsync(Student student);
        Task<Student?> UpdateAsync(int id, Student student);
        Task<bool> DeleteAsync(int id);
    }
}
