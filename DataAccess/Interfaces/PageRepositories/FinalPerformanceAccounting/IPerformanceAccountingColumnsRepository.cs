using Domain.Entities.JournalContent.FinalPerformanceAccounting;

namespace DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting
{
    public interface IPerformanceAccountingColumnsRepository : IPageRepositoryBase
    {
        Task<List<PerformanceAccountingColumn>?> GetByPageIdAsync(int pageId);
        Task<PerformanceAccountingColumn?> CreateAsync(PerformanceAccountingColumn column);
        Task<PerformanceAccountingColumn?> UpdateAsync(int id, PerformanceAccountingColumn column);
        Task<bool> DeleteAsync(int id);
    }
}
