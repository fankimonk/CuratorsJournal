using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface ISpecialtiesRepository
    {
        Task<List<Specialty>> GetAllAsync();
        Task<Specialty?> GetByIdAsync(int id);
        Task<Specialty?> CreateAsync(Specialty student);
        Task<Specialty?> UpdateAsync(int id, Specialty student);
        Task<bool> DeleteAsync(int id);
    }
}
