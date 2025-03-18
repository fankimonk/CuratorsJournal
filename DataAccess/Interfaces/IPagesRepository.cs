using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace DataAccess.Interfaces
{
    public interface IPagesRepository
    {
        Task<Page?> CreateAsync(Page page);
        Task<bool> DeleteAsync(int id);
        Task<List<Page>?> GetByJournalId(int journalId);
        Task<List<PageType>?> GetByJournalIdGroupedByTypes(int journalId);
        Task<Page?> GetById(int id);
        Task<List<Page>?> GetJournalPagesByType(int journalId, PageTypes pageType);
    }
}
