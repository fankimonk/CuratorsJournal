using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces
{
    public interface IGroupActivesRepository : IPageRepositoryBase
    {
        Task<List<GroupActive>?> GetByPageIdAsync(int pageId);
        Task<GroupActive?> CreateAsync(GroupActive active);
        Task<GroupActive?> UpdateAsync(int id, GroupActive active);
        Task<bool> DeleteAsync(int id);
    }
}
