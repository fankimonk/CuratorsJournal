using Domain.Entities.JournalContent;

namespace DataAccess.Interfaces.PageRepositories
{
    public interface ISocioPedagogicalCharacteristicsRepository : IPageRepositoryBase
    {
        Task<SocioPedagogicalCharacteristics?> GetByPageIdAsync(int pageId);
        Task<SocioPedagogicalCharacteristics?> CreateAsync(SocioPedagogicalCharacteristics characteristics);
        Task<SocioPedagogicalCharacteristics?> UpdateAsync(int id, SocioPedagogicalCharacteristics characteristics);
        Task<bool> DeleteAsync(int id);
    }
}
