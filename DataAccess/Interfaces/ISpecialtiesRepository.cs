using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface ISpecialtiesRepository
    {
        Task<List<Specialty>> GetAllAsync();
        Task<Specialty?> GetByIdAsync(int id);
    }
}
