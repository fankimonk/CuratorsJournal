using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IFacultiesRepository
    {
        Task<List<Faculty>> GetAllAsync();
        Task<Faculty?> CreateAsync(Faculty faculty);
        Task<Faculty?> UpdateAsync(int id, Faculty faculty);
        Task<bool> DeleteAsync(int id);
    }
}
