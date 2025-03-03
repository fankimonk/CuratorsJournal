using Domain.Entities.JournalContent.Pages;

namespace DataAccess.Interfaces
{
    public interface IPagesRepository
    {
        Task<bool> DeleteAsync(int id);
        Task<List<Page>> GetByJournalId(int journalId);
        Task<Page?> GetById(int id);
    }
}
