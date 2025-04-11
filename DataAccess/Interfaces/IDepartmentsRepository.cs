using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IDepartmentsRepository
    {
        Task<List<Department>?> GetAllAsync(int userId);
        Task<Department?> CreateAsync(Department position);
        Task<Department?> UpdateAsync(int id, Department position);
        Task<bool> DeleteAsync(int id);
    }
}
