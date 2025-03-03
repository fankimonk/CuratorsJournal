using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces
{
    public interface IEducationalProcessScheduleRepository : IPageRepositoryBase
    {
        Task<List<EducationalProcessScheduleRecord>?> GetByPageIdAsync(int pageId);
        Task<EducationalProcessScheduleRecord?> CreateAsync(EducationalProcessScheduleRecord record);
        Task<EducationalProcessScheduleRecord?> UpdateAsync(int id, EducationalProcessScheduleRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
