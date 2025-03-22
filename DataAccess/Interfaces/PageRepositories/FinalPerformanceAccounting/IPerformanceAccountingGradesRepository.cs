using Domain.Entities.JournalContent.FinalPerformanceAccounting;

namespace DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting
{
    public interface IPerformanceAccountingGradesRepository
    {
        Task<List<PerformanceAccountingGrade>?> GetByRecordIdAsync(int recordId);
        Task<List<PerformanceAccountingGrade>?> GetByColumnIdAsync(int columnId);
        Task<PerformanceAccountingGrade?> UpdateAsync(int id, PerformanceAccountingGrade grade);
    }
}
