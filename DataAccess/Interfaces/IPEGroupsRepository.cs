using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IPEGroupsRepository
    {
        Task<List<PEGroup>> GetAllAsync();
        Task<PEGroup?> CreateAsync(PEGroup peGroup);
        Task<PEGroup?> UpdateAsync(int id, PEGroup peGroup);
        Task<bool> DeleteAsync(int id);
    }
}
