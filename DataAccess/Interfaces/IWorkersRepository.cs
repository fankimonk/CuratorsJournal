using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IWorkersRepository
    {
        Task<List<Worker>> GetAllAsync(int? positionId);
        Task<Worker?> GetById(int id);
        Task<Worker?> CreateAsync(Worker worker);
        Task<Worker?> UpdateAsync(int id, Worker worker);
        Task<bool> DeleteAsync(int id);
    }
}
