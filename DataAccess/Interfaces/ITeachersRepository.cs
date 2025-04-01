using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface ITeachersRepository
    {
        Task<List<Teacher>> GetAllAsync();
        Task<Teacher?> GetByIdAsync(int id);
        Task<Teacher?> CreateAsync(Teacher teacher);
        Task<Teacher?> UpdateAsync(int id, Teacher teacher);
        Task<bool> DeleteAsync(int id);
    }
}
