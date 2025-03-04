using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface ITraditionsRepository : IPageRepositoryBase
    {
        Task<List<Tradition>?> GetByPageIdAsync(int pageId);
        Task<Tradition?> CreateAsync(Tradition tradition);
        Task<Tradition?> UpdateAsync(int id, Tradition tradition);
        Task<bool> DeleteAsync(int id);
    }
}
