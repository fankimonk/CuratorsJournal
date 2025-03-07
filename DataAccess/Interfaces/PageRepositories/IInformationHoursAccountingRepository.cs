using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IInformationHoursAccountingRepository : IPageRepositoryBase
    {
        Task<List<InformationHoursAccountingRecord>?> GetByPageIdAsync(int pageId);
        Task<InformationHoursAccountingRecord?> CreateAsync(InformationHoursAccountingRecord record);
        Task<InformationHoursAccountingRecord?> UpdateAsync(int id, InformationHoursAccountingRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
