using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IDynamicsOfKeyIndicatorsRepository : IPageRepositoryBase
    {
        Task<List<DynamicsOfKeyIndicatorsRecord>?> GetByPageIdAsync(int pageId);
        //Task<KeyIndicatorByCourse?> AddValueAsync(KeyIndicatorByCourse record);
        Task<List<DynamicsOfKeyIndicatorsRecord>?> AddCourseAsync(int pageId);
        Task<KeyIndicatorByCourse?> UpdateValueAsync(int id, KeyIndicatorByCourse record);
        Task<DynamicsOfKeyIndicatorsRecord?> UpdateAsync(int id, DynamicsOfKeyIndicatorsRecord record);
        Task<bool> DeleteAsync(int id);
        Task<List<DynamicsOfKeyIndicatorsRecord>?> DeleteCourseAsync(int pageId);
        //Task<bool> DeleteValueAsync(int id);
    }
}
