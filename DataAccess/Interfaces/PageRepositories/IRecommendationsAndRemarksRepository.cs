using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface IRecommendationsAndRemarksRepository : IPageRepositoryBase
    {
        Task<List<RecomendationsAndRemarksRecord>?> GetByPageIdAsync(int pageId);
        Task<RecomendationsAndRemarksRecord?> CreateAsync(RecomendationsAndRemarksRecord record);
        Task<RecomendationsAndRemarksRecord?> UpdateAsync(int id, RecomendationsAndRemarksRecord record);
        Task<bool> DeleteAsync(int id);
    }
}
