using Domain.Entities.JournalContent.Literature;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface ILiteratureWorkRepository : IPageRepositoryBase
    {
        Task<List<LiteratureWorkRecord>?> GetByPageIdAsync(int pageId);
        Task<LiteratureWorkRecord?> CreateAsync(LiteratureWorkRecord record);
        Task<LiteratureWorkRecord?> UpdateAsync(int id, LiteratureWorkRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
