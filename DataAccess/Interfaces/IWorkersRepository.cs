using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IWorkersRepository
    {
        Task<List<Worker>> GetAllAsync();
        Task<Worker?> GetById(int id);
    }
}
