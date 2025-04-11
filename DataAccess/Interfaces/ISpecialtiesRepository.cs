using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface ISpecialtiesRepository
    {
        Task<List<Specialty>?> GetAllAsync(int userId);
        Task<Specialty?> CreateAsync(Specialty student);
        Task<Specialty?> UpdateAsync(int id, Specialty student);
        Task<bool> DeleteAsync(int id);
    }
}
