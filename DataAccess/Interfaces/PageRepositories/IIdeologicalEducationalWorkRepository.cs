using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IIdeologicalEducationalWorkRepository : IPageRepositoryBase
    {
        Task<List<CuratorsIdeologicalAndEducationalWorkAccountingRecord>?> GetByPageIdAsync(int pageId);
        Task<CuratorsIdeologicalAndEducationalWorkAccountingRecord?> CreateAsync(CuratorsIdeologicalAndEducationalWorkAccountingRecord record);
        Task<CuratorsIdeologicalAndEducationalWorkAccountingRecord?> UpdateAsync(int id, CuratorsIdeologicalAndEducationalWorkAccountingRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
