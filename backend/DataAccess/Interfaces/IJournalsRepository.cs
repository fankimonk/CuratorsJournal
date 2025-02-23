using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IJournalsRepository
    {
        Task<List<Journal>> GetAllAsync();
        Task<Journal?> CreateAsync(Journal journal);
        Task<Journal?> GetJournalWithTitlePageData(int journalId);
    }
}
