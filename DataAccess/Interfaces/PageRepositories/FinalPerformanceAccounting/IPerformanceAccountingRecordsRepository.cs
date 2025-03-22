using Domain.Entities.JournalContent.FinalPerformanceAccounting;

namespace DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting
{
    public interface IPerformanceAccountingRecordsRepository : IPageRepositoryBase
    {
        Task<List<FinalPerformanceAccountingRecord>?> GetByPageIdAsync(int pageId);
        Task<FinalPerformanceAccountingRecord?> CreateAsync(FinalPerformanceAccountingRecord record);
        Task<FinalPerformanceAccountingRecord?> UpdateAsync(int id, FinalPerformanceAccountingRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
