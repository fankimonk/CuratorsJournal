using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IDynamicsOfKeyIndicatorsRepository : IPageRepositoryBase
    {
        Task<List<DynamicsOfKeyIndicatorsRecord>?> GetByPageIdAsync(int pageId);
        Task<KeyIndicatorByCourse?> AddValueAsync(KeyIndicatorByCourse record);
        Task<KeyIndicatorByCourse?> UpdateValueAsync(int id, KeyIndicatorByCourse record);
        Task<DynamicsOfKeyIndicatorsRecord?> UpdateAsync(int id, DynamicsOfKeyIndicatorsRecord record);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteValueAsync(int id);
    }
}
