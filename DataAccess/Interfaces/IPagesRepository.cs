using Domain.Entities.JournalContent.Pages;
using Domain.Enums.Journal;

namespace DataAccess.Interfaces
{
    public interface IPagesRepository
    {
        Task<Page?> CreateAsync(Page page);
        Task<bool> DeleteAsync(int id);
        Task<List<Page>?> GetByJournalIdAsync(int journalId);
        Task<List<PageType>?> GetByJournalIdGroupedByTypesAsync(int journalId);
        Task<Page?> GetByIdAsync(int id);
        Task<Page?> GetPageDataByIdAsync(int id);
        Task<List<Page>?> GetJournalPagesByTypeAsync(int journalId, PageTypes pageType);
        Task<bool?> ToggleIsApprovedAsync(int pageId);
        Task<bool> ApproveAllJournalPagesAsync(int journalId);
        Task<bool> UnapproveAllJournalPagesAsync(int journalId);
    }
}
