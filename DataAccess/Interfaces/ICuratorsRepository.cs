using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface ICuratorsRepository
    {
        Task<List<Curator>> GetAllAsync();
        Task<Curator?> GetByIdAsync(int id);
    }
}
