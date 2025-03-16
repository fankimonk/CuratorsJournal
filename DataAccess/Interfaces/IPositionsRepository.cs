using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IPositionsRepository
    {
        Task<List<Position>> GetAllAsync();
        Task<Position?> CreateAsync(Position position);
        Task<Position?> UpdateAsync(int id, Position position);
        Task<bool> DeleteAsync(int id);
    }
}
