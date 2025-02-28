using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces
{
    public interface IPagesRepository
    {
        Task<bool> DeleteAsync(int id);
        Task<List<Page>> GetByJournalId(int journalId);
        Task<Page?> GetById(int id);
    }
}
