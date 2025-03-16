using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IDeaneriesRepository
    {
        Task<List<Deanery>> GetAllAsync();
        Task<Deanery?> CreateAsync(Deanery position);
        Task<Deanery?> UpdateAsync(int id, Deanery position);
        Task<bool> DeleteAsync(int id);
    }
}
