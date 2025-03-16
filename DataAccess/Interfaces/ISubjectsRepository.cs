using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface ISubjectsRepository
    {
        Task<List<Subject>> GetAllAsync();
        Task<Subject?> CreateAsync(Subject subject);
        Task<Subject?> UpdateAsync(int id, Subject subject);
        Task<bool> DeleteAsync(int id);
    }
}
