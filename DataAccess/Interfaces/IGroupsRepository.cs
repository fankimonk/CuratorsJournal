using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IGroupsRepository
    {
        Task<List<Group>> GetAllAsync();
        Task<Group?> GetByIdAsync(int id);
        Task<Group?> GetByJournalId(int id);
        Task<Group?> CreateAsync(Group group);
        Task<Group?> UpdateAsync(int id, Group group);
        Task<Group?> UpdateCuratorAsync(int id, int? curatorId);
        Task<bool> DeleteAsync(int id);
    }
}
